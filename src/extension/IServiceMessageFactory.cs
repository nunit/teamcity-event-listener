namespace NUnit.Engine.Listeners
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Xml;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal interface IServiceMessageFactory
    {
        IEnumerable<ServiceMessage> SuiteStarted(EventId eventId, Event testEvent);

        IEnumerable<ServiceMessage> SuiteFinished(EventId eventId, Event testEvent);

        IEnumerable<ServiceMessage> FlowStarted(string flowId, string parentFlowId);

        IEnumerable<ServiceMessage> FlowFinished(string flowId);

        IEnumerable<ServiceMessage> TestStarted(EventId eventId);

        IEnumerable<ServiceMessage> TestFinished(EventId eventId, XmlNode testEvent, XmlNode infoEvent);

        IEnumerable<ServiceMessage> TestOutput(EventId eventId, XmlNode testEvent);

        IEnumerable<ServiceMessage> TestOutputAsMessage(EventId eventId, XmlNode testEvent);
    }
}