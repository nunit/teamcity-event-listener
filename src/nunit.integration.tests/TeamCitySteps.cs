using System.Text;

namespace nunit.integration.tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using System.Xml.XPath;

    using JetBrains.TeamCity.ServiceMessages;

    using Dsl;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class TeamCitySteps
    {
        [Given(@"I want to use (.+) type of TeamCity integration")]
        public void UseIntegrationType(string teamCityIntegration)
        {
            var ctx = ScenarioContext.Current.GetTestContext();
            var configuration = ctx.GetOrCreateNUnitConfiguration();
            switch (teamCityIntegration.ConvertToTeamCityIntegration())
            {
                case TeamCityIntegration.CmdArguments:
                    configuration.AddArg(new CmdArg(DataType.TeamCity));
                    break;

                case TeamCityIntegration.EnvVariable:
                    configuration.AddEnvVariable(new EnvVariable(DataType.TeamCity));
                    break;

                default:
                    throw new NotSupportedException(teamCityIntegration);
            }
        }

        [Then(@"the output should contain ([\d]+) TeamCity service messages")]
        public void ResultShouldContainNumberServiceMessage(int expectedNumberOfServiceMessages)
        {
            var ctx = ScenarioContext.Current.GetTestContext();
            var actualMessages = new TeamCityServiceMessageParser().Parse(ctx.TestSession.Output).ToList();
            Assert.AreEqual(expectedNumberOfServiceMessages, actualMessages.Count);
        }

        [Then(@"the output should contain TeamCity service messages:")]
        public void ResultShouldContainServiceMessage(Table data)
        {
            var ctx = ScenarioContext.Current.GetTestContext();
            var messages = new TeamCityServiceMessageParser().Parse(ctx.TestSession.Output).ToList();
            Assert.AreEqual(data.RowCount, messages.Count, $"{ctx}\nExpected number of service messages is {data.RowCount} but actual is {messages.Count}");

            var invalidMessages = (
                from item in data.Rows.Zip(messages, (row, message) => new { row, message })
                where !VerifyServiceMessage(item.row, item.message)
                select item).ToList();

            if (invalidMessages.Any())
            {
                var details = string.Join("\n", invalidMessages.Select(i => CreateErrorMessage(i.row, i.message)));
                Assert.Fail($"See {ctx}\n{details}");
            }
        }

        [Then(@"the output should contain correct set of TeamCity service messages")]
        public void ResultShouldContainCorrectStructureAndSequence()
        {
            var ctx = ScenarioContext.Current.GetTestContext();
            var messages = new TeamCityServiceMessageParser().Parse(ctx.TestSession.Output).ToList();
            var rootFlow = new Flow(string.Empty);
            foreach (var serviceMessage in messages)
            {
                rootFlow.ProcessMessage(new Message(serviceMessage));
            }
        }

        private class Message
        {
            public Message(IServiceMessage message)
            {
                if (message == null) throw new ArgumentNullException("message");

                Name = message.Name;

                FlowIdAttr = message.GetValue("flowId");
                Assert.IsNotEmpty(FlowIdAttr);

                NameAttr = message.GetValue("name");
                ParentAttr = message.GetValue("parent");
                CaptureStandardOutputAttr = message.GetValue("captureStandardOutput");
                DurationAttr = message.GetValue("duration");
                OutAttr = message.GetValue("duration");
                MessageAttr = message.GetValue("message");
                TextAttr = message.GetValue("text");
                DetailsAttr = message.GetValue("details");
                TcTagsAttr = message.GetValue("tc:tags");
                TestNameAttr = message.GetValue("testName");
                TypeAttr = message.GetValue("type");
                ValueAttr = message.GetValue("value");
            }

            public string Name { get; }

            public string FlowIdAttr { get; }

            public string NameAttr { get; }

            public string ParentAttr { get; }

            public string CurrentFlowId => ParentAttr ?? FlowIdAttr;

            public string CaptureStandardOutputAttr { get; }

            public string DurationAttr { get; }

            public string OutAttr { get; }

            public string MessageAttr { get; }

            public string TextAttr { get; }

            public string DetailsAttr { get; }

            public string TcTagsAttr { get; }

            public string TestNameAttr { get; }

            public string TypeAttr { get; }

            public string ValueAttr { get; }
        }

        private class Flow
        {
            private readonly Stack<Message> _messages = new Stack<Message>();
            private readonly List<Message> _allMessages = new List<Message>();
            private readonly List<Flow> _flows = new List<Flow>();
            private readonly string _parentFlowId;

            public Flow(string flowId, Flow parentFlow = null)
            {
                FlowId = flowId;
                _parentFlowId = parentFlow?.FlowId ?? string.Empty;
            }

            public string FlowId { get; }

            public bool IsFinished => _messages.Count == 0;

            public void ProcessMessage(Message message)
            {
                if (message.FlowIdAttr == FlowId)
                {
                    ProcessMessageInternal(message);
                    return;
                }

                var flow = _flows.SingleOrDefault(i => i.FlowId == message.FlowIdAttr);
                if (flow == null)
                {
                    flow = new Flow(message.FlowIdAttr, this);
                    _flows.Add(flow);
                }

                flow.ProcessMessage(message);

                if (flow.IsFinished)
                {
                    _flows.Remove(flow);
                }
            }

            private void ProcessMessageInternal(Message message)
            {
                _allMessages.Add(message);
                switch (message.Name)
                {
                    case "testSuiteStarted":
                        Assert.IsNotEmpty(message.FlowIdAttr, "FlowId attribute is empty" + GetDetails());
                        Assert.IsNotEmpty(message.ParentAttr, "Parent attribute is empty" + GetDetails());
                        Assert.IsNotEmpty(message.NameAttr, "Name attribute is empty" + GetDetails());
                        _messages.Push(message);
                        break;

                    case "testSuiteFinished":
                        Assert.IsNotEmpty(message.FlowIdAttr, "FlowId attribute is empty" + GetDetails());
                        Assert.IsNotEmpty(message.ParentAttr, "Parent attribute is empty" + GetDetails());
                        var testSuiteStarted = _messages.Pop();
                        Assert.AreEqual(testSuiteStarted.Name, "testSuiteStarted", "testSuiteFinished should close testSuiteStarted" + GetDetails());
                        Assert.AreEqual(testSuiteStarted.FlowIdAttr, message.FlowIdAttr, "Invalid FlowId attribute" + GetDetails());
                        Assert.AreEqual(testSuiteStarted.NameAttr, message.NameAttr, "Invalid Name attribute" + GetDetails());
                        break;

                    case "flowStarted":
                        Assert.IsNotEmpty(message.FlowIdAttr, "Invalid FlowId attribute" + GetDetails());
                        if (_parentFlowId != string.Empty)
                        {
                            Assert.AreEqual(message.ParentAttr, _parentFlowId, "Invalid Parent attribute" + GetDetails());
                        }

                        _messages.Push(message);
                        break;

                    case "flowFinished":
                        Assert.AreEqual(message.FlowIdAttr, FlowId, "Invalid FlowId attribute" + GetDetails());
                        Assert.Greater(_messages.Count, 0, "flowFinished should close flowStarted" + GetDetails());
                        var flowStarted = _messages.Pop();
                        Assert.AreEqual(flowStarted.Name, "flowStarted", "flowFinished should close flowStarted" + GetDetails());
                        break;

                    case "testStarted":
                        Assert.AreEqual(message.FlowIdAttr, FlowId, "Invalid FlowId attribute" + GetDetails());
                        Assert.IsNotEmpty(message.NameAttr, "Name attribute is empty" + GetDetails());
                        Assert.AreEqual(message.CaptureStandardOutputAttr, "false", "Invalid CaptureStandardOutput attribute" + GetDetails());
                        _messages.Push(message);
                        break;

                    case "testFinished":
                        Assert.AreEqual(message.FlowIdAttr, FlowId, "Invalid FlowId attribute" + GetDetails());
                        Assert.IsNotEmpty(message.NameAttr, "Name attribute is empty" + GetDetails());
                        Assert.Greater(_messages.Count, 0, "testFinished should close testStarted" + GetDetails());
                        var testStarted = _messages.Pop();
                        Assert.AreEqual(testStarted.Name, "testStarted", "testFinished should close testStarted" + GetDetails());
                        Assert.AreEqual(testStarted.NameAttr, message.NameAttr, "Invalid Name attribute" + GetDetails());
                        Assert.IsNotEmpty(message.DurationAttr, "Duration attribute is empty" + GetDetails());
                        break;

                    case "testStdOut":
                    case "testStdErr":
                        Assert.AreEqual(message.FlowIdAttr, FlowId, "Invalid FlowId attribute" + GetDetails());
                        Assert.IsNotEmpty(message.NameAttr, "Name attribute is empty" + GetDetails());
                        Assert.Greater(_messages.Count, 0, "testStdOut should be within testStarted and testFinished" + GetDetails());
                        var testStartedForStdOut = _messages.Peek();
                        Assert.AreEqual(testStartedForStdOut.NameAttr, message.NameAttr, "Invalid Name attribute" + GetDetails());
                        Assert.IsNotEmpty(message.OutAttr, "Out attribute is empty" + GetDetails());
                        Assert.IsNotEmpty(message.TcTagsAttr, "tc:tags should be tc:parseServiceMessagesInside" + GetDetails());
                        break;

                    case "testFailed":
                        Assert.AreEqual(message.FlowIdAttr, FlowId, "Invalid FlowId attribute" + GetDetails());
                        Assert.IsNotEmpty(message.NameAttr, "Name attribute is empty" + GetDetails());
                        Assert.Greater(_messages.Count, 0, "testFailed should be within testStarted and testFinished" + GetDetails());
                        var testStartedForTestFailed = _messages.Peek();
                        Assert.AreEqual(testStartedForTestFailed.Name, "testStarted", "testFailed should be within testStarted and testFinished" + GetDetails());
                        Assert.AreEqual(testStartedForTestFailed.NameAttr, message.NameAttr, "Invalid Name attribute" + GetDetails());
                        Assert.IsNotEmpty(message.MessageAttr, "Message attribute is empty" + GetDetails());
                        Assert.IsNotNull(message.DetailsAttr, "Details attribute is empty" + GetDetails());
                        break;

                    case "testIgnored":
                        Assert.AreEqual(message.FlowIdAttr, FlowId, "Invalid FlowId attribute" + GetDetails());
                        Assert.IsNotEmpty(message.NameAttr, "Name attribute is empty" + GetDetails());
                        Assert.Greater(_messages.Count, 0, "testIgnored should be within testStarted and testFinished" + GetDetails());
                        var testStartedForTestIgnored = _messages.Pop();
                        Assert.AreEqual(testStartedForTestIgnored.Name, "testStarted", "testIgnored should be within testStarted and testFinished" + GetDetails());
                        Assert.AreEqual(testStartedForTestIgnored.NameAttr, message.NameAttr, "Invalid Name attribute" + GetDetails());
                        Assert.IsNotEmpty(message.MessageAttr, "Message attribute is empty" + GetDetails());
                        break;

                    case "message":
                        Assert.AreEqual(message.FlowIdAttr, FlowId, "Invalid FlowId attribute" + GetDetails());
                        Assert.IsNotEmpty(message.TextAttr, "Text attribute is empty" + GetDetails());
                        Assert.IsNotEmpty(message.TcTagsAttr, "tc:tags should be tc:parseServiceMessagesInside" + GetDetails());
                        break;

                    case "publishArtifacts":
                        Assert.AreEqual(message.FlowIdAttr, FlowId, "Invalid FlowId attribute" + GetDetails());
                        break;

                    case "testMetadata":
                        Assert.AreEqual(message.FlowIdAttr, FlowId, "Invalid FlowId attribute" + GetDetails());
                        Assert.IsNotEmpty(message.TestNameAttr, "TestName attribute is empty" + GetDetails());
                        Assert.IsNotEmpty(message.TypeAttr, "Type attribute is empty" + GetDetails());
                        Assert.IsNotEmpty(message.ValueAttr, "Value attribute is empty" + GetDetails());
                        Assert.Greater(_messages.Count, 0, "testMetadata should be after testStarted" + GetDetails());
                        testStarted = _messages.Peek();
                        Assert.AreEqual(testStarted.NameAttr, message.TestNameAttr, "Invalid TestName attribute" + GetDetails());
                        break;

                    default:
                        Assert.Fail($"Unexpected message {message.Name}" + GetDetails());
                        break;
                }
            }

            private string GetDetails()
            {
                var sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine($"Messages ({_allMessages.Count}):");
                sb.AppendLine(string.Join("\n", _allMessages.Select(i => i.ToString())));
                return sb.ToString();
            }
        }

        [Given(@"I created assemblies according to NUnit2 test results (.+)")]
        public void CreateAssemblies(string xmlReportFileName)
        {
            var ctx = ScenarioContext.Current.GetTestContext();
            xmlReportFileName = Path.Combine(ctx.SandboxPath, xmlReportFileName);

            using (var xmlReportFile = File.OpenRead(xmlReportFileName))
            {
                var doc = XDocument.Load(xmlReportFile);

                var tests =
                    from assemblyElement in doc.XPathSelectElements("//test-suite[@type='Assembly' and @name]")
                    let assemblyName = Path.GetFileNameWithoutExtension(assemblyElement.Attribute("name").Value)
                    from testFixtureElement in assemblyElement.XPathSelectElements(".//test-suite[@type='TestFixture' and @name]")
                    let testFixtureName = testFixtureElement.Attribute("name").Value
                    let testFixtureNamespace = string.Join(".", GetNamespaces(testFixtureElement).Reverse())
                    let testFixtureFullName = string.Join(".", Enumerable.Repeat(testFixtureNamespace, 1).Concat(Enumerable.Repeat(testFixtureName, 1)).Where(i => !string.IsNullOrWhiteSpace(i)))
                    from testElement in testFixtureElement.XPathSelectElements(".//test-case[@name]")
                    let fullTestName = testElement.Attribute("name").Value
                    where fullTestName.Length > testFixtureFullName.Length + 1
                    let testName = fullTestName.Substring(testFixtureFullName.Length + 1, fullTestName.Length - testFixtureFullName.Length - 1)
                    let result = testElement.Attribute("result")?.Value ?? string.Empty
                    select new { testName, testFixtureNamespace, testFixtureFullName, assemblyName, result };

                var nunitSteps = new NUnitSteps();
                foreach (var test in tests)
                {
                    switch (test.result.ToLowerInvariant())
                    {
                        case "success":
                            nunitSteps.AddMethod("Successful", test.testName, test.testFixtureNamespace, test.testFixtureFullName, test.assemblyName);
                            break;

                        case "inconclusive":
                            nunitSteps.AddMethod("Inconclusive", test.testName, test.testFixtureNamespace, test.testFixtureFullName, test.assemblyName);
                            break;

                        case "ignored":
                            nunitSteps.AddMethod("Ignored", test.testName, test.testFixtureNamespace, test.testFixtureFullName, test.assemblyName);
                            break;

                        case "failed":
                            nunitSteps.AddMethod("Failed", test.testName, test.testFixtureNamespace, test.testFixtureFullName, test.assemblyName);
                            break;
                    }
                }
            }
        }

        private static IEnumerable<string> GetNamespaces(XElement element)
        {
            if (element == null)
            {
                yield break;
            }

            if (element.Name == "test-suite" && StringComparer.InvariantCultureIgnoreCase.Equals(element.Attribute("type")?.Value ?? string.Empty, "Namespace"))
            {
                yield return element.Attribute("name")?.Value ?? string.Empty;
            }

            if (element.Parent == null)
            {
                yield break;
            }

            var parentNs = GetNamespaces(element.Parent).ToList();
            foreach (var parentNamespace in parentNs)
            {
                yield return parentNamespace;
            }
        }

        private static bool VerifyServiceMessage(TableRow row, IServiceMessage serviceMessage)
        {
            var messageNameRegex = new Regex(row[""]);
            if (!messageNameRegex.IsMatch(serviceMessage.Name))
            {
                return false;
            }

            foreach (var key in row.Keys)
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }

                string rowValue;
                if (!row.TryGetValue(key, out rowValue))
                {
                    continue;
                }

                string serviceMessageValue;
                if (key == ".")
                {
                    serviceMessageValue = serviceMessage.DefaultValue ?? "";
                }
                else
                {
                    serviceMessageValue = serviceMessage.GetValue(key) ?? "";
                }

                var rowValueRegex = new Regex(rowValue ?? "");
                if (!rowValueRegex.IsMatch(serviceMessageValue.Replace("\n", " ").Replace("\r", " ")))
                {
                    return false;
                }
            }

            return true;
        }

        private static string CreateErrorMessage(TableRow row, IServiceMessage serviceMessage)
        {
            var rowInfo = string.Join(", ", from key in row.Keys select $"{key} = {row[key]}");
            var serviceMessageInfo = string.Join(", ", from key in serviceMessage.Keys select $"{key} = {serviceMessage.GetValue(key)}");            

            return $"Expected service message should has:\n{rowInfo}\nbut it has:\n{serviceMessageInfo}";
        }
    }
}