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
    using System.Collections.Specialized;
    using System.IO;

    internal class EventConverter3 : IEventConverter
    {
        private readonly IServiceMessageFactory _serviceMessageFactory;
        private readonly IHierarchy _hierarchy;
        private readonly Statistics _statistics;
        private readonly TextWriter _outWriter;
        private readonly ITeamCityInfo _teamCityInfo;

        private readonly Dictionary<string, List<EventId>> _testSuiteTestEvents =
            new Dictionary<string, List<EventId>>();

        private readonly Dictionary<string, XmlNode> _notStartedNUnit3Tests = new Dictionary<string, XmlNode>();
        private readonly Dictionary<string, string> _suiteAssembly = new Dictionary<string, string>();

        public EventConverter3(IServiceMessageFactory serviceMessageFactory, IHierarchy hierarchy,
            Statistics statistics, ITeamCityInfo teamCityInfo, TextWriter outWriter)
        {
            if (serviceMessageFactory == null) throw new ArgumentNullException("serviceMessageFactory");
            if (hierarchy == null) throw new ArgumentNullException("hierarchy");
            if (statistics == null) throw new ArgumentNullException("statistics");
            if (teamCityInfo == null) throw new ArgumentNullException("teamCityInfo");

            _serviceMessageFactory = serviceMessageFactory;
            _hierarchy = hierarchy;
            _statistics = statistics;
            _teamCityInfo = teamCityInfo;
            _outWriter = outWriter;
        }

        public IEnumerable<IEnumerable<ServiceMessage>> Convert(Event testEvent)
        {
            if (testEvent.MessageName == "start-run")
            {
                _hierarchy.Clear();
                _suiteAssembly.Clear();
                _notStartedNUnit3Tests.Clear();
                _testSuiteTestEvents.Clear();
                yield break;
            }

            var id = testEvent.Id;
            var parentId = testEvent.ParentId;

            string rootId;
            var flowId = _hierarchy.TryFindRootId(parentId, out rootId) ? rootId : id;
            var testFlowId = id != flowId ? id : flowId;
            var rootFlowId = testEvent.RootFlowId;
            if (string.IsNullOrEmpty(rootFlowId))
            {
                rootFlowId = ".";
            }

            var testEventId = new EventId(_teamCityInfo, testFlowId, testEvent.FullName);

            var eventId = new EventId(_teamCityInfo, flowId, testEvent.FullName);
            switch (testEvent.MessageName)
            {
                case "start-suite":
                    _hierarchy.AddLink(id, parentId);

                    // Root
                    if (parentId == string.Empty)
                    {
                        //Save assembly name
                        _suiteAssembly[flowId] = testEvent.Name;

                        // Start a flow from a root flow https://youtrack.jetbrains.com/issue/TW-56310
                        yield return _serviceMessageFactory.FlowStarted(flowId, rootFlowId);

                        _statistics.RegisterSuiteStart();
                        yield return _serviceMessageFactory.SuiteStarted(eventId, testEvent);
                    }

                    break;

                case "test-suite":
                    _hierarchy.AddLink(id, parentId);
                    if (_suiteAssembly.ContainsKey(rootFlowId))
                    {
                        _suiteAssembly[flowId] = _suiteAssembly[rootFlowId];
                    }

                    yield return ProcessNotStartedTests(flowId, id, testEvent.TestEvent);
                    yield return ProcessTestSuiteProperties(flowId, id, testEvent.TestEvent);
                    yield return _serviceMessageFactory.TestOutputAsMessage(eventId, testEvent.TestEvent);

                    // Root
                    if (parentId == string.Empty)
                    {
                        _statistics.RegisterSuiteFinish();
                        yield return _serviceMessageFactory.SuiteFinished(eventId, testEvent);

                        // Finish a child flow from a root flow https://youtrack.jetbrains.com/issue/TW-56310
                        yield return _serviceMessageFactory.FlowFinished(flowId);
                    }

                    break;

                case "start-test":
                    List<EventId> existingEventList;
                    if (_testSuiteTestEvents.TryGetValue(parentId, out existingEventList))
                    {
                        existingEventList.Add(testEventId);
                    }
                    else
                    {
                        _testSuiteTestEvents[parentId] = new List<EventId>() { testEventId };
                    }

                    _hierarchy.AddLink(id, parentId);
                    if (testFlowId != eventId.FlowId)
                    {
                        yield return _serviceMessageFactory.FlowStarted(testFlowId, eventId.FlowId);
                    }

                    _statistics.RegisterTestStart();
                    yield return _serviceMessageFactory.TestStarted(new EventId(_teamCityInfo, testFlowId,
                        eventId.FullName));
                    break;

                case "test-case":
                    if (_hierarchy.AddLink(id, parentId))
                    {
                        // When a test without starting
                        _notStartedNUnit3Tests[testFlowId] = testEvent.TestEvent;
                        break;
                    }

                    _statistics.RegisterTestFinish();
                    yield return _serviceMessageFactory.TestFinished(testEventId, testEvent.TestEvent,
                        testEvent.TestEvent);

                    if (id != flowId && parentId != null)
                    {
                        yield return _serviceMessageFactory.FlowFinished(id);
                    }

                    break;

                case "test-run":
                    yield return ProcessNotStartedTests(flowId, id, testEvent.TestEvent);
                    break;

                case "test-output":
                    yield return _serviceMessageFactory.TestOutput(testEventId, testEvent.TestEvent);
                    break;
            }
        }

        private IEnumerable<ServiceMessage> ProcessTestSuiteProperties(string flowId, string suiteId,
            XmlNode testSuiteNode)
        {
            var properties = testSuiteNode.SelectNodes("properties/property");
            List<EventId> testEventIds;

            if (_testSuiteTestEvents.TryGetValue(suiteId, out testEventIds) && properties != null)
            {
                var props = new NameValueCollection();
                foreach (var property in properties)
                {
                    var propertyElement = property as XmlNode;
                    if (propertyElement == null)
                    {
                        continue;
                    }

                    var propertyName = propertyElement.GetAttribute("name") ?? string.Empty;
                    var propertyValue = propertyElement.GetAttribute("value") ?? string.Empty;

                    props.Add(propertyName, propertyValue);
                }

                if (testEventIds.Count > 0)
                {
                    foreach (var eventId in testEventIds)
                    {
                        foreach (var name in props.AllKeys)
                        {
                            string assemblyName;
                            var dllName = _suiteAssembly.TryGetValue(flowId, out assemblyName) ? assemblyName : "";
                            var testFullName = dllName == "" ? eventId.FullName : dllName + ": " + eventId.FullName;
                            var attrs = new List<ServiceMessageAttr>
                            {
                                new ServiceMessageAttr(ServiceMessageAttr.Names.TestName, testFullName),
                                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, name),
                                new ServiceMessageAttr(ServiceMessageAttr.Names.Value, props[name])
                            };
                            yield return new ServiceMessage(ServiceMessage.Names.TestMetadata, attrs);
                        }
                    }
                }
            }
        }

        private IEnumerable<ServiceMessage> ProcessNotStartedTests(string flowId, string id, XmlNode currentEvent)
        {
            var testToProcess = new List<string>();
            foreach (var notStartedTest in _notStartedNUnit3Tests)
            {
                var parentId = notStartedTest.Key;
                while (_hierarchy.TryFindParentId(parentId, out parentId))
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

                foreach (var message in
                         _serviceMessageFactory.TestStarted(new EventId(_teamCityInfo, flowId, fullName)))
                {
                    _statistics.RegisterTestStart();
                    yield return message;
                }

                foreach (var message in _serviceMessageFactory.TestFinished(
                             new EventId(_teamCityInfo, flowId, fullName), testEvent, currentEvent))
                {
                    _statistics.RegisterTestFinish();
                    yield return message;
                }
            }
        }
    }
}