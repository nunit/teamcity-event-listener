namespace NUnit.Engine.Listeners
{
    using System.Collections.Generic;
    using System.Xml;

    internal interface IServiceMessageFactory
    {
        IEnumerable<ServiceMessage> SuiteStarted(EventId eventId);

        IEnumerable<ServiceMessage> SuiteFinished(EventId eventId);

        IEnumerable<ServiceMessage> FlowStarted(string flowId, string parentFlowId);

        IEnumerable<ServiceMessage> FlowFinished(string flowId);

        IEnumerable<ServiceMessage> TestStarted(EventId eventId);

        IEnumerable<ServiceMessage> TestFinished(EventId eventId, XmlNode testEvent, XmlNode infoEvent);

        IEnumerable<ServiceMessage> TestOutput(EventId eventId, XmlNode testEvent);

        IEnumerable<ServiceMessage> TestOutputAsMessage(EventId eventId, XmlNode testEvent);
    }
}