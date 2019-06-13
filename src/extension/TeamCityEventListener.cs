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
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System;
    using System.IO;
    using System.Xml;
    using Extensibility;

    // Note: Setting minimum engine version in this case is
    // purely documentary since engines prior to 3.4 do not
    // check the EngineVersion property and will try to
    // load this extension anyway.
    [Extension(Enabled = false, EngineVersion = "3.4")]
    [SuppressMessage("ReSharper", "UseNameofExpression")]
    public class TeamCityEventListener : ITestEventListener
    {
        private readonly IServiceMessageWriter _serviceMessageWriter;
        private readonly IEventConverter _eventConverter2;
        private readonly IEventConverter _eventConverter3;
        private readonly Statistics _statistics = new Statistics();
        private readonly ITeamCityInfo _teamCityInfo;
        private readonly object _lockObject = new object();
        private readonly TextWriter _outWriter;
        private string _rootFlowId = string.Empty;        

        // ReSharper disable once UnusedMember.Global
        public TeamCityEventListener() : this(Console.Out, new TeamCityInfo()) { }

        public TeamCityEventListener(TextWriter outWriter, ITeamCityInfo teamCityInfo)
        {
            if (outWriter == null) throw new ArgumentNullException("outWriter");
            if (teamCityInfo == null) throw new ArgumentNullException("teamCityInfo");

            _outWriter = outWriter;
            _teamCityInfo = teamCityInfo;

            _serviceMessageWriter = new ServiceMessageWriter();
            var serviceMessageFactory = new ServiceMessageFactory(_teamCityInfo, new SuiteNameReplacer(_teamCityInfo));
            var hierarchy =  new Hierarchy();
            _eventConverter2 = new EventConverter2(serviceMessageFactory, hierarchy, _statistics, _teamCityInfo);
            _eventConverter3 = new EventConverter3(serviceMessageFactory, hierarchy, _statistics, _teamCityInfo);
            RootFlowId = _teamCityInfo.RootFlowId;            
        }

        public string RootFlowId
        {
            set
            {
                _rootFlowId = value ?? string.Empty;
            }
        }

        public void OnTestEvent(string report)
        {
            if (_teamCityInfo.AllowDiagnostics)
            {
                _outWriter.WriteLine();
                _outWriter.WriteLine("PID_" + _teamCityInfo.ProcessId + " !!!!{ " + report + " }!!!!");
            }

            var doc = new XmlDocument();
            doc.LoadXml(report);

            var testEvent = doc.FirstChild;
            RegisterMessage(testEvent);
        }

        public void RegisterMessage(XmlNode xmlEvent)
        {
            if (xmlEvent == null) throw new ArgumentNullException("xmlEvent");
            var messageName = xmlEvent.Name;
            if (string.IsNullOrEmpty(messageName))
            {
                return;
            }
            
            var fullName = xmlEvent.GetAttribute("fullname");
            if (string.IsNullOrEmpty(fullName))
            {
                fullName = xmlEvent.GetAttribute("testname");
                if (string.IsNullOrEmpty(fullName))
                {
                    return;
                }
            }

            var name = xmlEvent.GetAttribute("name");
            if (string.IsNullOrEmpty(name))
            {
                name = fullName;
            }

            var id = xmlEvent.GetAttribute("id") ?? string.Empty;
            var parentId = xmlEvent.GetAttribute("parentId");
            var testId = xmlEvent.GetAttribute("testid");

            var isNUnit3 = parentId != null;
            var eventConverter = isNUnit3 ? _eventConverter3 : _eventConverter2;
            var testEvent = new Event(_rootFlowId, messageName.ToLowerInvariant(), fullName, name, GetId(_rootFlowId, id), GetId(_rootFlowId, parentId), GetId(_rootFlowId, testId), xmlEvent);
            lock (_lockObject)
            {
                var sb = new StringBuilder();
                using (var writer = new StringWriter(sb))
                {
                    foreach (var messages in eventConverter.Convert(testEvent))
                    {
                        _serviceMessageWriter.Write(writer, messages);
                    }
                }

                _outWriter.Write(sb.ToString());
            }

            if (_teamCityInfo.AllowDiagnostics)
            {
                _outWriter.WriteLine("@@ NUnit3: " + isNUnit3 + ", " + _statistics + ", " + testEvent);
            }
        }

        private static string GetId(string rootFlowId, string flowId)
        {
            if (string.IsNullOrEmpty(flowId) || string.IsNullOrEmpty(rootFlowId))
            {
                return flowId;
            }

            return rootFlowId + "_" + flowId;
        }
    }
}
