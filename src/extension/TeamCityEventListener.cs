// ***********************************************************************
// Copyright (c) 2015 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Engine.Extensibility;

namespace NUnit.Engine.Listeners
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    // Note: Setting minimum engine version in this case is
    // purely documentary since engines prior to 3.4 do not
    // check the EngineVersion property and will try to
    // load this extension anyway.
    [Extension(Enabled = false, EngineVersion = "3.4")]
    [SuppressMessage("ReSharper", "UseNameofExpression")]
    public class TeamCityEventListener : ITestEventListener
    {
        private static readonly ServiceMessageWriter ServiceMessageWriter = new ServiceMessageWriter();
        private readonly TextWriter _outWriter;
        private readonly Dictionary<string, string> _refs = new Dictionary<string, string>();
        private readonly Dictionary<string, int> _blockCounters = new Dictionary<string, int>();
        private readonly Dictionary<string, XmlNode> _notStartedNUnit3Tests = new Dictionary<string, XmlNode>();
        private readonly List<XmlNode> _notStartedNUnit2Tests = new List<XmlNode>();
        private string _rootFlowId;

        // ReSharper disable once UnusedMember.Global
        public TeamCityEventListener() : this(Console.Out) { }

        public string RootFlowId
        {
            private get { return _rootFlowId; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _rootFlowId = ".";
                    return;
                }

                _rootFlowId = value;
            }
        }

        public TeamCityEventListener(TextWriter outWriter)
        {
            if (outWriter == null) throw new ArgumentNullException("outWriter");

            _outWriter = outWriter;
            RootFlowId = Environment.GetEnvironmentVariable("TEAMCITY_PROCESS_FLOW_ID");
        }

        #region ITestEventListener Implementation

        public void OnTestEvent(string report)
        {
            var doc = new XmlDocument();
            doc.LoadXml(report);

            var testEvent = doc.FirstChild;
            RegisterMessage(testEvent);
        }

        #endregion

        public void RegisterMessage(XmlNode testEvent)
        {
            if (testEvent == null) throw new ArgumentNullException("testEvent");

            var messageName = testEvent.Name;
            // Console.WriteLine(testEvent.OuterXml);
            if (string.IsNullOrEmpty(messageName))
            {
                return;
            }

            messageName = messageName.ToLowerInvariant();
            if (messageName == "start-run")
            {
                _refs.Clear();
                _notStartedNUnit3Tests.Clear();
                _notStartedNUnit2Tests.Clear();
                return;
            }

            var fullName = testEvent.GetAttribute("fullname");
            if (string.IsNullOrEmpty(fullName))
            {
                fullName = testEvent.GetAttribute("testname");
                if (string.IsNullOrEmpty(fullName))
                {
                    return;
                }
            }

            var id = testEvent.GetAttribute("id") ?? testEvent.GetAttribute("testid") ?? string.Empty;
            var parentId = testEvent.GetAttribute("parentId");
            var flowId = RootFlowId;
            var isNUnit3 = parentId != null;

            if (isNUnit3)
            {
                // NUnit 3 case
                string rootId;
                flowId = TryFindRootId(parentId, out rootId) ? rootId : id;
            }
            else
            {
                // NUnit 2 case
                if (!string.IsNullOrEmpty(id))
                {
                    var idParts = id.Split('-');
                    if (idParts.Length == 2)
                    {
                        flowId = idParts[0];
                    }
                }
            }

            string testFlowId;
            if (id != flowId && isNUnit3)
            {
                testFlowId = id;
            }
            else
            {
                testFlowId = flowId ?? id;
            }

            switch (messageName.ToLowerInvariant())
            {
                case "start-suite":
                    AddParent(id, parentId);

                    // NUnit 3 case
                    if (parentId == string.Empty)
                    {
                        // Start the flow from the root flow
                        // https://youtrack.jetbrains.com/issue/TW-56310
                        OnFlowStarted(flowId, RootFlowId);
                    }

                    StartSuiteCase(parentId, flowId, fullName);
                    break;

                case "test-suite":
                    AddParent(id, parentId);
                    ProcessNotStartedTests(isNUnit3, id, flowId, testEvent);
                    TestSuiteCase(parentId, flowId, fullName, testEvent);

                    // NUnit 3 case
                    if (parentId == string.Empty)
                    {
                        // Finish the child flow from the root flow
                        // https://youtrack.jetbrains.com/issue/TW-56310
                        OnFlowFinished(flowId);
                    }

                    break;

                case "start-test":
                    AddParent(id, parentId);
                    if (isNUnit3)
                    {
                        CaseStartTest(id, flowId, parentId, testFlowId, fullName);
                    }

                    break;

                case "test-case":
                    if (isNUnit3)
                    {
                        if (!_refs.ContainsKey(id))
                        {
                            _refs[id] = parentId;

                            // When test without starting
                            _notStartedNUnit3Tests[testFlowId] = testEvent;
                            break;
                        }
                    }
                    else
                    {
                        var errorMessage = testEvent.SelectSingleNode("failure/message");
                        if (errorMessage != null && errorMessage.InnerText.StartsWith("TestFixtureSetUp failed in", StringComparison.CurrentCultureIgnoreCase))
                        {
                            _notStartedNUnit2Tests.Add(testEvent);
                            break;
                        }

                        CaseStartTest(id, flowId, null, testFlowId, fullName);
                    }

                    try
                    {
                        OnTestCase(testEvent, testEvent, testFlowId, fullName);
                    }
                    finally
                    {
                        if (id != flowId && parentId != null)
                        {
                            OnFlowFinished(id);
                        }
                    }

                    break;

                case "test-run":
                    ProcessNotStartedTests(isNUnit3, id, flowId, testEvent);
                    break;

                case "test-output":
                    TestOutput(testEvent, id, fullName);
                    break;
            }
        }

        private void AddParent(string id, string parentId)
        {
            _refs[id] = parentId;
        }

        private void TestOutput(XmlNode testEvent, string flowId, string fullName)
        {
            if (string.IsNullOrEmpty(flowId))
            {
                return;
            }

            var stream = testEvent.GetAttribute("stream");
            if (!string.IsNullOrEmpty(stream) && stream.ToLower() == "error")
            {
                SendStdErr(flowId, fullName, testEvent.InnerText);
            }
            else
            {
                SendStdOut(flowId, fullName, testEvent.InnerText);
            }
        }

        private void OnTestCase(XmlNode testEvent, XmlNode infoEvent, string testFlowId, string fullName)
        {
            var result = testEvent.GetAttribute("result");
            if (string.IsNullOrEmpty(result))
            {
                return;
            }

            switch (result.ToLowerInvariant())
            {
                case "passed":
                    OnTestFinished(testFlowId, testEvent, fullName);
                    break;

                case "inconclusive":
                    OnTestInconclusive(testFlowId, testEvent, fullName);
                    break;

                case "skipped":
                    OnTestSkipped(testFlowId, testEvent, fullName);
                    break;

                case "failed":
                    OnTestFailed(testFlowId, testEvent, fullName, infoEvent);
                    break;
            }
        }

        private void ProcessNotStartedTests(bool isNUnit3, string id, string flowId, XmlNode currentEvent)
        {
            if (isNUnit3)
            {
                var testToProcess = new List<string>();
                foreach (var notStartedTest in _notStartedNUnit3Tests)
                {
                    var parentId = notStartedTest.Key;
                    while (_refs.TryGetValue(parentId, out parentId))
                    {
                        if (id == parentId)
                        {
                            testToProcess.Add(notStartedTest.Key);
                            break;
                        }
                    }
                }

                foreach (var testId in testToProcess)
                {
                    var testEvent = _notStartedNUnit3Tests[testId];
                    _notStartedNUnit3Tests.Remove(testId);
                    var fullName = testEvent.GetAttribute("fullname");
                    if (string.IsNullOrEmpty(fullName))
                    {
                        continue;
                    }

                    OnTestStart(flowId, fullName);
                    OnTestCase(testEvent, currentEvent, flowId, fullName);
                }
            }
            else
            {
                foreach (var notStartedTest in _notStartedNUnit2Tests)
                {
                    var fullName = notStartedTest.GetAttribute("fullname");
                    if (string.IsNullOrEmpty(fullName))
                    {
                        continue;
                    }

                    OnTestStart(flowId, fullName);
                    OnTestCase(notStartedTest, currentEvent, flowId, fullName);
                }

                _notStartedNUnit2Tests.Clear();
            }
        }

        private void CaseStartTest(string id, string flowId, string parentId, string testFlowId, string fullName)
        {
            if (id != flowId && parentId != null)
            {
                OnFlowStarted(id, flowId);
            }

            OnTestStart(testFlowId, fullName);
        }

        private void TestSuiteCase(string parentId, string flowId, string fullName, XmlNode testEvent)
        {
            TrySendOutputAsMessage(flowId, testEvent);

            // NUnit 3 case
            if (parentId == string.Empty)
            {
                OnRootSuiteFinish(flowId, fullName);
            }

            // NUnit 2 case
            if (parentId == null)
            {
                if (ChangeBlockCounter(flowId, -1) == 0)
                {
                    OnRootSuiteFinish(flowId, fullName);
                }
            }
        }

        private void StartSuiteCase(string parentId, string flowId, string fullName)
        {
            // NUnit 3 case
            if (parentId == string.Empty)
            {
                OnRootSuiteStart(flowId, fullName);
            }

            // NUnit 2 case
            if (parentId == null)
            {
                if (ChangeBlockCounter(flowId, 1) == 1)
                {
                    OnRootSuiteStart(flowId, fullName);
                }
            }
        }

        private int ChangeBlockCounter(string flowId, int changeValue)
        {
            int currentBlockCounter;
            if (!_blockCounters.TryGetValue(flowId, out currentBlockCounter))
            {
                currentBlockCounter = 0;
            }

            currentBlockCounter += changeValue;
            _blockCounters[flowId] = currentBlockCounter;
            return currentBlockCounter;
        }

        private bool TryFindParentId(string id, out string parentId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            return _refs.TryGetValue(id, out parentId) && !string.IsNullOrEmpty(parentId);
        }

        private bool TryFindRootId(string id, out string rootId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            while (TryFindParentId(id, out rootId) && id != rootId)
            {
                id = rootId;
            }

            rootId = id;
            return !string.IsNullOrEmpty(id);
        }

        private void TrySendOutput(string flowId, XmlNode message, string fullName)
        {
            if (message == null) throw new ArgumentNullException("message");

            var output = message.SelectSingleNode("output");
            if (output == null)
            {
                return;
            }

            SendStdOut(flowId, fullName, output.InnerText);
        }

        private void TrySendOutputAsMessage(string flowId, XmlNode message)
        {
            if (message == null) throw new ArgumentNullException("message");

            var output = message.SelectSingleNode("output");
            if (output == null)
            {
                return;
            }

            SendOutputAsMessage(flowId, output.InnerText);
        }

        private void TrySendReasonMessage(string flowId, XmlNode message, string fullName)
        {
            if (message == null) throw new ArgumentNullException("message");

            var reasonMessageElement = message.SelectSingleNode("reason/message");
            if (reasonMessageElement == null)
            {
                return;
            }

            var reasonMessage = reasonMessageElement.InnerText;
            if (string.IsNullOrEmpty(reasonMessage))
            {
                return;
            }

            SendStdOut(flowId, fullName, "Assert.Pass message: " + reasonMessage);
        }

        private void SendStdOut(string flowId, string fullName, string outputStr)
        {
            SendOutput(ServiceMessage.Names.TestStdOut, flowId, fullName, outputStr);
        }

        private void SendStdErr(string flowId, string fullName, string outputStr)
        {
            SendOutput(ServiceMessage.Names.TestStdErr, flowId, fullName, outputStr);
        }

        private void SendOutput(string messageName, string flowId, string fullName, string outputStr)
        {
            if (string.IsNullOrEmpty(outputStr))
            {
                return;
            }

            Write(new ServiceMessage(messageName,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, fullName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Out, outputStr),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, flowId),
                new ServiceMessageAttr(ServiceMessageAttr.Names.TcTags, "tc:parseServiceMessagesInside")));
        }

        private void SendOutputAsMessage(string flowId, string outputStr)
        {
            if (string.IsNullOrEmpty(outputStr))
            {
                return;
            }

            Write(new ServiceMessage(ServiceMessage.Names.Message,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Message, outputStr),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, flowId),
                new ServiceMessageAttr(ServiceMessageAttr.Names.TcTags, "tc:parseServiceMessagesInside")));
        }

        private void OnRootSuiteStart(string flowId, string assemblyName)
        {
            assemblyName = Path.GetFileName(assemblyName);

            Write(new ServiceMessage(ServiceMessage.Names.TestSuiteStarted,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, assemblyName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, flowId)));
        }

        private void OnRootSuiteFinish(string flowId, string assemblyName)
        {
            assemblyName = Path.GetFileName(assemblyName);

            Write(new ServiceMessage(ServiceMessage.Names.TestSuiteFinished,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, assemblyName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, flowId)));
        }

        private void OnFlowStarted(string flowId, string parentFlowId)
        {
            Write(new ServiceMessage(ServiceMessage.Names.FlowStarted,
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, flowId),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Parent, parentFlowId)));
        }

        private void OnFlowFinished(string flowId)
        {
            Write(new ServiceMessage(ServiceMessage.Names.FlowFinished,
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, flowId)));
        }

        private void OnTestStart(string flowId, string fullName)
        {
            Write(new ServiceMessage(ServiceMessage.Names.TestStarted,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, fullName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.CaptureStandardOutput, "false"),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, flowId)));
        }

        private void OnTestFinished(string flowId, XmlNode message, string fullName)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var durationStr = message.GetAttribute(ServiceMessageAttr.Names.Duration);
            double durationDecimal;
            int durationMilliseconds = 0;
            if (durationStr != null && double.TryParse(durationStr, NumberStyles.Any, CultureInfo.InvariantCulture, out durationDecimal))
            {
                durationMilliseconds = (int)(durationDecimal * 1000d);
            }

            TrySendOutput(flowId, message, fullName);
            TrySendReasonMessage(flowId, message, fullName);

            Write(new ServiceMessage(ServiceMessage.Names.TestFinished,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, fullName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Duration, durationMilliseconds.ToString()),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, flowId)));
        }

        private void OnTestFailed(string flowId, XmlNode message, string fullName, XmlNode infoSource)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (infoSource == null)
            {
                infoSource = message;
            }

            var errorMessage = infoSource.SelectSingleNode("failure/message");
            var stackTrace = infoSource.SelectSingleNode("failure/stack-trace");

            Write(new ServiceMessage(ServiceMessage.Names.TestFailed,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, fullName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Message, errorMessage == null ? string.Empty : errorMessage.InnerText),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Details, stackTrace == null ? string.Empty : stackTrace.InnerText),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, flowId)));

            OnTestFinished(flowId, message, fullName);
        }

        private void OnTestSkipped(string flowId, XmlNode message, string fullName)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            TrySendOutput(flowId, message, fullName);
            var reason = message.SelectSingleNode("reason/message");

            Write(new ServiceMessage(ServiceMessage.Names.TestIgnored,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, fullName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Message, reason == null ? string.Empty : reason.InnerText),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, flowId)));
        }

        private void OnTestInconclusive(string flowId, XmlNode message, string fullName)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            TrySendOutput(flowId, message, fullName);

            Write(new ServiceMessage(ServiceMessage.Names.TestIgnored,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, fullName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Message, "Inconclusive"),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, flowId)));
        }

        private void Write(ServiceMessage serviceMessage)
        {
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                ServiceMessageWriter.Write(writer, serviceMessage);
            }

            _outWriter.WriteLine(sb.ToString());
        }
    }
}
