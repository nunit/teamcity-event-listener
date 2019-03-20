namespace NUnit.Engine.Listeners
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Xml;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class ServiceMessageFactory : IServiceMessageFactory
    {
        private readonly ITeamCityInfo _teamCityInfo;
        private readonly ISuiteNameReplacer _suiteNameReplacer;
        private const string TcParseServiceMessagesInside = "tc:parseServiceMessagesInside";
        private static readonly IEnumerable<ServiceMessage> EmptyServiceMessages = new ServiceMessage[0];
        private static readonly Regex AttachmentDescriptionRegex = new Regex("(.*)=>(.+)", RegexOptions.Compiled);
        private static readonly List<char> _invalidChars;

        static ServiceMessageFactory()
        {
            var invalidPathChars = Path.GetInvalidPathChars();
            var invalidFileNameChars = Path.GetInvalidFileNameChars();
            _invalidChars = new List<char>(invalidPathChars.Length + invalidFileNameChars.Length);
            foreach (var c in invalidPathChars)
            {
                _invalidChars.Add(c);
            }

            foreach (var c in invalidFileNameChars)
            {
                if (_invalidChars.Contains(c))
                {
                    continue;
                }

                _invalidChars.Add(c);
            }
        }

        public ServiceMessageFactory(ITeamCityInfo teamCityInfo, ISuiteNameReplacer suiteNameReplacer)
        {
            _teamCityInfo = teamCityInfo;
            _suiteNameReplacer = suiteNameReplacer;
        }

        public IEnumerable<ServiceMessage> SuiteStarted(EventId eventId, Event testEvent)
        {
            yield return new ServiceMessage(ServiceMessage.Names.TestSuiteStarted,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, _suiteNameReplacer.Replace(testEvent.Name)),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, eventId.FlowId));
        }

        public IEnumerable<ServiceMessage> SuiteFinished(EventId eventId, Event testEvent)
        {
            yield return new ServiceMessage(ServiceMessage.Names.TestSuiteFinished,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, _suiteNameReplacer.Replace(testEvent.Name)),
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

            if (_teamCityInfo.MetadataEnabled)
            {
                foreach (var message in Attachments(eventId, testEvent))
                {
                    yield return message;
                }
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

        private static IEnumerable<ServiceMessage> TestFinished(EventId eventId, XmlNode testFinishedEvent)
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

        private static IEnumerable<ServiceMessage> TestFailed(EventId eventId, XmlNode testFailedEvent, XmlNode infoSource)
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

        private static IEnumerable<ServiceMessage> TestSkipped(EventId eventId, XmlNode testSkippedEvent)
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

        private static IEnumerable<ServiceMessage> TestInconclusive(EventId eventId, XmlNode testInconclusiveEvent)
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

        private static IEnumerable<ServiceMessage> Output(EventId eventId, string messageName, string outputStr)
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

        private static IEnumerable<ServiceMessage> OutputAsMessage(EventId eventId, string outputStr)
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

        private static IEnumerable<ServiceMessage> Output(EventId eventId, XmlNode sendOutputEvent)
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

        private static IEnumerable<ServiceMessage> ReasonMessage(EventId eventId, XmlNode ev)
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

        private static IEnumerable<ServiceMessage> Attachments(EventId eventId, XmlNode testEvent)
        {
            var attachments = testEvent.SelectNodes("attachments/attachment");
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    var attachmentElement = attachment as XmlNode;
                    if (attachmentElement == null)
                    {
                        continue;
                    }

                    var filePathNode = attachmentElement.SelectSingleNode("filePath");
                    if (filePathNode != null)
                    {
                        var filePath = filePathNode.InnerText;
                        var descriptionNode = attachmentElement.SelectSingleNode("description");
                        string description = null;
                        if (descriptionNode != null)
                        {
                            description = descriptionNode.InnerText;
                        }

                        var fileName = Path.GetFileName(filePath);
                        var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

                        string artifactDir = null;
                        if (!string.IsNullOrEmpty(description))
                        {
                            var match = AttachmentDescriptionRegex.Match(description);
                            if (match.Success)
                            {
                                description = match.Groups[1].Value.Trim();
                                artifactDir = match.Groups[2].Value.Trim();
                            }
                        }

                        if (artifactDir == null)
                        {
                            var testDirNameChars = new char[eventId.FullName.Length];
                            eventId.FullName.CopyTo(0, testDirNameChars, 0, eventId.FullName.Length);
                            for (var i = 0; i < testDirNameChars.Length; i++)
                            {
                                if (_invalidChars.Contains(testDirNameChars[i]))
                                {
                                    testDirNameChars[i] = '_';
                                }
                            }

                            var testDirName = new string(testDirNameChars);
                            artifactDir = ".teamcity/NUnit/" + testDirName + "/" + Guid.NewGuid();
                        }

                        string artifactType;
                        switch (fileExtension)
                        {
                            case ".bmp":
                            case ".gif":
                            case ".ico":
                            case ".jng":
                            case ".jpeg":
                            case ".jpg":
                            case ".jfif":
                            case ".jp2":
                            case ".jps":
                            case ".tga":
                            case ".tiff":
                            case ".tif":
                            case ".svg":
                            case ".wmf":
                            case ".emf":
                            case ".png":
                                artifactType = "image";
                                break;

                            default:
                                artifactType = "artifact";
                                break;
                        }

                        yield return new ServiceMessage(ServiceMessage.Names.PublishArtifacts, filePath + " => " + artifactDir);

                        var attrs = new List<ServiceMessageAttr>
                        {
                            new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, eventId.FlowId),
                            new ServiceMessageAttr(ServiceMessageAttr.Names.TestName, eventId.FullName),
                            new ServiceMessageAttr(ServiceMessageAttr.Names.Type, artifactType),
                            new ServiceMessageAttr(ServiceMessageAttr.Names.Value, artifactDir + "/" + fileName)
                        };

                        if (!string.IsNullOrEmpty(description))
                        {
                            attrs.Add(new ServiceMessageAttr(ServiceMessageAttr.Names.Name, description));
                        }

                        yield return new ServiceMessage(ServiceMessage.Names.TestMetadata, attrs);
                    }
                }
            }
        }
    }
}
