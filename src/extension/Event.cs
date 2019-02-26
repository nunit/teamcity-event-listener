namespace NUnit.Engine.Listeners
{
    using System.Xml;

    public struct Event
    {
        public readonly string RootFlowId;
        public readonly string MessageName;
        public readonly string FullName;
        public readonly string Id;
        public readonly string ParentId;
        public readonly XmlNode TestEvent;

        public Event(
            string rootFlowId,
            string messageName,
            string fullName,
            string id,
            string parentId,
            XmlNode testEvent)
        {
            RootFlowId = rootFlowId;
            MessageName = messageName;
            FullName = fullName;
            Id = id;
            ParentId = parentId;
            TestEvent = testEvent;
        }

        public override string ToString()
        {
            return string.Format("RootFlowId: {0}, MessageName: {1}, FullName: {2}, Id: {3}, ParentId: {4}, Event: {5}", RootFlowId, MessageName, FullName, Id, ParentId, TestEvent.OuterXml);
        }
    }
}
