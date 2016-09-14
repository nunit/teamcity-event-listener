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

        [Then(@"the output should contain TeamCity service messages:")]
        public void ResultShouldContainServiceMessage(Table data)
        {
            var ctx = ScenarioContext.Current.GetTestContext();
            var messages = new TeamCityServiceMessageParser().Parse(ctx.TestSession.Output).ToList();
            Assert.AreEqual(data.RowCount, messages.Count, $"{ctx}\nExpected service messages are {data.RowCount} but actual is {messages.Count}");

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
            var rootFlows = new List<Flow>();
            foreach (var serviceMessage in messages)
            {
                var message = new Message(serviceMessage);
                var flow = rootFlows.SingleOrDefault(i => i.FlowId == message.CurrentFlowId);
                if (flow == null)
                {
                    flow = new Flow(message.FlowIdAttr);
                    rootFlows.Add(flow);
                }

                flow.ProcessMessage(message);

                if (flow.IsFinished)
                {
                    rootFlows.Remove(flow);
                }
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
                DetailsAttr = message.GetValue("details");
                TcTagsAttr = message.GetValue("tc:tags");
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

            public string DetailsAttr { get; }

            public string TcTagsAttr { get; }
        }

        private class Flow
        {
            private readonly Stack<Message> _messages = new Stack<Message>();

            public Flow(string flowId)
            {
                FlowId = flowId;
            }

            public string FlowId { get; private set; }

            public bool IsFinished => _messages.Count == 0;

            public void ProcessMessage(Message message)
            {
                switch (message.Name)
                {
                    case "testSuiteStarted":
                        Assert.AreEqual(_messages.Count, 0, "testSuiteStarted should be a first message");
                        Assert.IsNotEmpty(message.FlowIdAttr, "FlowId attribute is empty");
                        Assert.IsNotEmpty(message.NameAttr, "Name attribute is empty");
                        FlowId = message.FlowIdAttr;
                        _messages.Push(message);
                        break;

                    case "testSuiteFinished":
                        Assert.AreEqual(_messages.Count, 1, "testSuiteFinished should close testSuiteStarted");
                        var testSuiteStarted = _messages.Pop();
                        Assert.AreEqual(testSuiteStarted.Name, "testSuiteStarted", "testSuiteFinished should close testSuiteStarted");
                        Assert.AreEqual(testSuiteStarted.FlowIdAttr, message.FlowIdAttr, "Invalid FlowId attribute");
                        Assert.AreEqual(testSuiteStarted.NameAttr, message.NameAttr, "Invalid Name attribute");
                        break;

                    case "flowStarted":
                        Assert.IsNotEmpty(message.FlowIdAttr, "Invalid FlowId attribute");
                        Assert.AreEqual(message.ParentAttr, FlowId, "Invalid Parent attribute");
                        FlowId = message.FlowIdAttr;
                        _messages.Push(message);
                        break;

                    case "flowFinished":
                        Assert.AreEqual(message.FlowIdAttr, FlowId, "Invalid FlowId attribute");
                        Assert.Greater(_messages.Count, 1, "flowFinished should close flowStarted");
                        var flowStarted = _messages.Pop();
                        Assert.AreEqual(flowStarted.Name, "flowStarted", "flowFinished should close flowStarted");
                        FlowId = flowStarted.ParentAttr;
                        break;

                    case "testStarted":
                        Assert.AreEqual(message.FlowIdAttr, FlowId, "Invalid FlowId attribute");
                        Assert.IsNotEmpty(message.NameAttr, "Name attribute is empty");
                        Assert.AreEqual(message.CaptureStandardOutputAttr, "false", "Invalid CaptureStandardOutput attribute");
                        _messages.Push(message);
                        break;

                    case "testFinished":
                        Assert.AreEqual(message.FlowIdAttr, FlowId, "Invalid FlowId attribute");
                        Assert.IsNotEmpty(message.NameAttr, "Name attribute is empty");
                        Assert.Greater(_messages.Count, 1, "testFinished should close testStarted");
                        var testStarted = _messages.Pop();
                        Assert.AreEqual(testStarted.Name, "testStarted", "testFinished should close testStarted");
                        Assert.AreEqual(testStarted.NameAttr, message.NameAttr, "Invalid Name attribute");
                        Assert.IsNotEmpty(message.DurationAttr, "Duration attribute is empty");
                        break;

                    case "testStdOut":
                        Assert.AreEqual(message.FlowIdAttr, FlowId, "Invalid FlowId attribute");
                        Assert.IsNotEmpty(message.NameAttr, "Name attribute is empty");
                        Assert.Greater(_messages.Count, 1, "testStdOut should be within testStarted and testFinished");
                        var testStartedForStdOut = _messages.Peek();
                        Assert.AreEqual(testStartedForStdOut.Name, "testStarted", "testStdOut should be within testStarted and testFinished");
                        Assert.AreEqual(testStartedForStdOut.NameAttr, message.NameAttr, "Invalid Name attribute");
                        Assert.IsNotEmpty(message.OutAttr, "Out attribute is empty");
                        Assert.IsNotEmpty(message.TcTagsAttr, "tc:tags should be tc:parseServiceMessagesInside");
                        break;

                    case "testFailed":
                        Assert.AreEqual(message.FlowIdAttr, FlowId, "Invalid FlowId attribute");
                        Assert.IsNotEmpty(message.NameAttr, "Name attribute is empty");
                        Assert.Greater(_messages.Count, 1, "testFailed should be within testStarted and testFinished");
                        var testStartedForTestFailed = _messages.Peek();
                        Assert.AreEqual(testStartedForTestFailed.Name, "testStarted", "testFailed should be within testStarted and testFinished");
                        Assert.AreEqual(testStartedForTestFailed.NameAttr, message.NameAttr, "Invalid Name attribute");
                        Assert.IsNotEmpty(message.MessageAttr, "Message attribute is empty");
                        Assert.IsNotNull(message.DetailsAttr, "Details attribute is empty");
                        break;

                    case "testIgnored":
                        Assert.AreEqual(message.FlowIdAttr, FlowId, "Invalid FlowId attribute");
                        Assert.IsNotEmpty(message.NameAttr, "Name attribute is empty");
                        Assert.Greater(_messages.Count, 1, "testIgnored should be within testStarted and testFinished");
                        var testStartedForTestIgnored = _messages.Pop();
                        Assert.AreEqual(testStartedForTestIgnored.Name, "testStarted", "testIgnored should be within testStarted and testFinished");
                        Assert.AreEqual(testStartedForTestIgnored.NameAttr, message.NameAttr, "Invalid Name attribute");
                        Assert.IsNotEmpty(message.MessageAttr, "Message attribute is empty");
                        break;

                    default:
                        Assert.Fail($"Unexpected message {message.Name}");
                        break;
                }
            }
        }

        [Given(@"I have created assemblies according to NUnit2 test results (.+)")]
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

                var serviceMessageValue = serviceMessage.GetValue(key) ?? "";
                var rowValueRegex = new Regex(rowValue ?? "");
                if (!rowValueRegex.IsMatch(serviceMessageValue))
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