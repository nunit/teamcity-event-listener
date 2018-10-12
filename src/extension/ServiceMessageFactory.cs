namespace NUnit.Engine.Listeners
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Xml;

    internal class ServiceMessageFactory : IServiceMessageFactory
    {
        private const string TcParseServiceMessagesInside = "tc:parseServiceMessagesInside";
        private static readonly IEnumerable<ServiceMessage> EmptyServiceMessages = new ServiceMessage[0];

        public IEnumerable<ServiceMessage> SuiteStarted(EventId eventId)
        {
            var assemblyName = Path.GetFileName(eventId.FullName);

            yield return new ServiceMessage(ServiceMessage.Names.TestSuiteStarted,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, assemblyName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, eventId.FlowId));
        }

        public IEnumerable<ServiceMessage> SuiteFinished(EventId eventId)
        {
            var assemblyName = Path.GetFileName(eventId.FullName);

            yield return new ServiceMessage(ServiceMessage.Names.TestSuiteFinished,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, assemblyName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, eventId.FlowId));
        }

        public IEnumerable<ServiceMessage> FlowStarted(string flowId, string parentFlowId)
        {
            yield return new ServiceMessage(ServiceMessage.Names.FlowStarted,
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, flowId),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Parent, parentFlowId));
        }

        public IEnumerable<ServiceMessage> FlowFinished(string flowId)
        {
            yield return new ServiceMessage(ServiceMessage.Names.FlowFinished,
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, flowId));
        }

        public IEnumerable<ServiceMessage> TestStarted(EventId eventId)
        {
            yield return new ServiceMessage(ServiceMessage.Names.TestStarted,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, eventId.FullName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.CaptureStandardOutput, "false"),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, eventId.FlowId));
        }

        public IEnumerable<ServiceMessage> TestFinished(EventId eventId, XmlNode testEvent, XmlNode infoEvent)
        {
            var result = testEvent.GetAttribute("result");
            if (string.IsNullOrEmpty(result))
            {
                yield break;
            }

            IEnumerable<ServiceMessage> messages;
            switch (result.ToLowerInvariant())
            {
                case "passed":
                    messages = TestFinished(eventId, testEvent);
                    break;

                case "inconclusive":
                    messages = TestInconclusive(eventId, testEvent);
                    break;

                case "skipped":
                    messages = TestSkipped(eventId, testEvent);
                    break;

                case "failed":
                    messages = TestFailed(eventId, testEvent, infoEvent);
                    break;

                default:
                    messages = EmptyServiceMessages;
                    break;
            }

            foreach (var message in messages)
            {
                yield return message;
            }
        }

        public IEnumerable<ServiceMessage> TestOutput(EventId eventId, XmlNode testEvent)
        {
            if (string.IsNullOrEmpty(eventId.FlowId))
            {
                yield break;
            }

            var stream = testEvent.GetAttribute("stream");
            IEnumerable<ServiceMessage> messages;
            if (!string.IsNullOrEmpty(stream) && stream.ToLower() == "error")
            {
                messages = Output(eventId, ServiceMessage.Names.TestStdErr, testEvent.InnerText);
            }
            else
            {
                messages = Output(eventId, ServiceMessage.Names.TestStdOut, testEvent.InnerText);
            }

            foreach (var message in messages)
            {
                yield return message;
            }
        }

        public IEnumerable<ServiceMessage> TestOutputAsMessage(EventId eventId, XmlNode testEvent)
        {
            if (testEvent == null) throw new ArgumentNullException("testEvent");

            var output = testEvent.SelectSingleNode("output");
            if (output == null)
            {
                yield break;
            }

            foreach (var message in OutputAsMessage(eventId, output.InnerText))
            {
                yield return message;
            }
        }

        private IEnumerable<ServiceMessage> TestFinished(EventId eventId, XmlNode testFinishedEvent)
        {
            if (testFinishedEvent == null)
            {
                throw new ArgumentNullException("testFinishedEvent");
            }

            var durationStr = testFinishedEvent.GetAttribute(ServiceMessageAttr.Names.Duration);
            double durationDecimal;
            var durationMilliseconds = 0;
            if (durationStr != null && double.TryParse(durationStr, NumberStyles.Any, CultureInfo.InvariantCulture, out durationDecimal))
            {
                durationMilliseconds = (int)(durationDecimal * 1000d);
            }

            foreach (var message in Output(eventId, testFinishedEvent))
            {
                yield return message;
            }

            foreach (var message in ReasonMessage(eventId, testFinishedEvent))
            {
                yield return message;
            }

            yield return new ServiceMessage(ServiceMessage.Names.TestFinished,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, eventId.FullName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Duration, durationMilliseconds.ToString()),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, eventId.FlowId));
        }

        private IEnumerable<ServiceMessage> TestFailed(EventId eventId, XmlNode testFailedEvent, XmlNode infoSource)
        {
            if (testFailedEvent == null)
            {
                throw new ArgumentNullException("testFailedEvent");
            }

            if (infoSource == null)
            {
                infoSource = testFailedEvent;
            }

            var errorMessage = infoSource.SelectSingleNode("failure/message");
            var stackTrace = infoSource.SelectSingleNode("failure/stack-trace");

            yield return new ServiceMessage(ServiceMessage.Names.TestFailed,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, eventId.FullName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Message, errorMessage == null ? string.Empty : errorMessage.InnerText),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Details, stackTrace == null ? string.Empty : stackTrace.InnerText),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, eventId.FlowId));

            foreach (var message in TestFinished(eventId, testFailedEvent))
            {
                yield return message;
            }
        }

        private IEnumerable<ServiceMessage> TestSkipped(EventId eventId, XmlNode testSkippedEvent)
        {
            if (testSkippedEvent == null)
            {
                throw new ArgumentNullException("testSkippedEvent");
            }

            foreach (var message in Output(eventId, testSkippedEvent))
            {
                yield return message;
            }

            var reason = testSkippedEvent.SelectSingleNode("reason/message");

            yield return new ServiceMessage(ServiceMessage.Names.TestIgnored,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, eventId.FullName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Message, reason == null ? string.Empty : reason.InnerText),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, eventId.FlowId));
        }

        private IEnumerable<ServiceMessage> TestInconclusive(EventId eventId, XmlNode testInconclusiveEvent)
        {
            if (testInconclusiveEvent == null)
            {
                throw new ArgumentNullException("testInconclusiveEvent");
            }

            foreach (var message in Output(eventId, testInconclusiveEvent))
            {
                yield return message;
            }

            yield return new ServiceMessage(ServiceMessage.Names.TestIgnored,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, eventId.FullName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Message, "Inconclusive"),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, eventId.FlowId));
        }

        private IEnumerable<ServiceMessage> Output(EventId eventId, string messageName, string outputStr)
        {
            if (string.IsNullOrEmpty(outputStr))
            {
                yield break;
            }

            yield return new ServiceMessage(messageName,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, eventId.FullName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Out, outputStr),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, eventId.FlowId),
                new ServiceMessageAttr(ServiceMessageAttr.Names.TcTags, TcParseServiceMessagesInside));
        }

        private IEnumerable<ServiceMessage> OutputAsMessage(EventId eventId, string outputStr)
        {
            if (string.IsNullOrEmpty(outputStr))
            {
                yield break;
            }

            yield return new ServiceMessage(ServiceMessage.Names.Message,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Text, outputStr),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, eventId.FlowId),
                new ServiceMessageAttr(ServiceMessageAttr.Names.TcTags, TcParseServiceMessagesInside));
        }

        private IEnumerable<ServiceMessage> Output(EventId eventId, XmlNode sendOutputEvent)
        {
            if (sendOutputEvent == null) throw new ArgumentNullException("sendOutputEvent");

            var output = sendOutputEvent.SelectSingleNode("output");
            if (output == null)
            {
                yield break;
            }

            foreach (var message in Output(eventId, ServiceMessage.Names.TestStdOut, output.InnerText))
            {
                yield return message;
            }
        }

        private IEnumerable<ServiceMessage> ReasonMessage(EventId eventId, XmlNode ev)
        {
            if (ev == null) throw new ArgumentNullException("ev");

            var reasonMessageElement = ev.SelectSingleNode("reason/message");
            if (reasonMessageElement == null)
            {
                yield break;
            }

            var reasonMessage = reasonMessageElement.InnerText;
            if (string.IsNullOrEmpty(reasonMessage))
            {
                yield break;
            }

            foreach (var message in Output(eventId, ServiceMessage.Names.TestStdOut, "Assert.Pass message: " + reasonMessage))
            {
                yield return message;
            }
        }
    }
}
