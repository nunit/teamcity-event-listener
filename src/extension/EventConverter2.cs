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

namespace NUnit.Engine.Listeners
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    internal class EventConverter2: IEventConverter
    {
        private readonly IServiceMessageFactory _serviceMessageFactory;
        private readonly IHierarchy _hierarchy;

        private readonly Dictionary<string, int> _blockCounters = new Dictionary<string, int>();
        private readonly List<XmlNode> _notStartedNUnit2Tests = new List<XmlNode>();

        public EventConverter2(IServiceMessageFactory serviceMessageFactory, IHierarchy hierarchy)
        {
            if (serviceMessageFactory == null) throw new ArgumentNullException("serviceMessageFactory");
            if (hierarchy == null) throw new ArgumentNullException("hierarchy");
            _serviceMessageFactory = serviceMessageFactory;
            _hierarchy = hierarchy;
        }

        public IEnumerable<IEnumerable<ServiceMessage>> Convert(Event testEvent)
        {
            if (testEvent.MessageName == "start-run")
            {
                _hierarchy.Clear();
                _notStartedNUnit2Tests.Clear();
                yield break;
            }
            
            var rootFlowId = testEvent.RootFlowId;
            var id = testEvent.Id;
            var flowId = rootFlowId;

            if (!string.IsNullOrEmpty(id))
            {
                var idParts = id.Split('-');
                if (idParts.Length == 2)
                {
                    if (!string.IsNullOrEmpty(flowId))
                    {
                        flowId += ".";
                    }

                    flowId += idParts[0];
                }
            }

            if (string.IsNullOrEmpty(flowId))
            {
                flowId = ".";
            }

            var testFlowId = flowId ?? id;

            var eventId = new EventId(flowId, testEvent.FullName);
            switch (testEvent.MessageName)
            {
                case "start-suite":
                    _hierarchy.AddLink(id, testEvent.ParentId);

                    if (ChangeBlockCounter(flowId, 1) == 1)
                    {
                        yield return _serviceMessageFactory.SuiteStarted(eventId);
                    }

                    break;

                case "test-suite":
                    _hierarchy.AddLink(id, testEvent.ParentId);
                    yield return ProcessNotStartedTests(flowId, testEvent.TestEvent);
                    yield return _serviceMessageFactory.TestOutputAsMessage(eventId, testEvent.TestEvent);

                   
                    if (ChangeBlockCounter(flowId, -1) == 0)
                    {
                        yield return _serviceMessageFactory.SuiteFinished(eventId);
                    }

                    break;

                case "start-test":
                    _hierarchy.AddLink(id, testEvent.ParentId);
                    break;

                case "test-case":
                    var errorMessage = testEvent.TestEvent.SelectSingleNode("failure/message");
                    if (errorMessage != null && errorMessage.InnerText.StartsWith("TestFixtureSetUp failed in", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _notStartedNUnit2Tests.Add(testEvent.TestEvent);
                        break;
                    }

                    var testEventId = new EventId(testFlowId, testEvent.FullName);
                    yield return _serviceMessageFactory.TestStarted(testEventId);
                    yield return _serviceMessageFactory.TestFinished(testEventId, testEvent.TestEvent, testEvent.TestEvent);

                    break;

                case "test-run":
                    yield return ProcessNotStartedTests(flowId, testEvent.TestEvent);
                    break;

                case "test-output":
                    testFlowId = testEvent.TestEvent.GetAttribute("testid") ?? rootFlowId;
                    yield return _serviceMessageFactory.TestOutput(new EventId(testFlowId, testEvent.FullName), testEvent.TestEvent);
                    break;
            }
        }

        private IEnumerable<ServiceMessage> ProcessNotStartedTests(string flowId, XmlNode currentEvent)
        {
            foreach (var notStartedTest in _notStartedNUnit2Tests)
            {
                var fullName = notStartedTest.GetAttribute("fullname");
                if (string.IsNullOrEmpty(fullName))
                {
                    continue;
                }

                foreach (var message in _serviceMessageFactory.TestStarted(new EventId(flowId, fullName)))
                {
                    yield return message;
                }

                foreach (var message in _serviceMessageFactory.TestFinished(new EventId(flowId, fullName), notStartedTest, currentEvent))
                {
                    yield return message;
                }
            }

            _notStartedNUnit2Tests.Clear();
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
    }
}
