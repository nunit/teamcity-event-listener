namespace NUnit.Engine.Listeners
{
  using System;
  using System.Xml;

    public struct Event
    {
        public readonly string RootFlowId;
        public readonly string MessageName;
        public readonly string FullName;
        public readonly string Name;
        public readonly string Type;
        public readonly string Id;
        public readonly string ParentId;
        // ReSharper disable once InconsistentNaming
        public readonly string TestId;
        // ReSharper disable once InconsistentNaming
        public readonly XmlNode TestEvent;

        public Event(
            string rootFlowId,
            string messageName,
            string fullName,
            string name,
            string id,
            string parentId,
            string testId,
            string type,
            XmlNode testEvent)
        {
            RootFlowId = rootFlowId;
            MessageName = messageName;
            FullName = fullName;
            Name = name;
            Id = id;
            ParentId = parentId;
            TestId = testId;
            Type = type;
            TestEvent = testEvent;
        }

        public override string ToString()
        {
            return string.Format("RootFlowId: {0}, MessageName: {1}, FullName: {2}, Id: {3}, ParentId: {4}, Type: {5}, TestEvent.OuterXml: {6}{7}", 
              RootFlowId, MessageName, FullName, Id, ParentId, Type, Environment.NewLine, TestEvent.OuterXml);
        }
    }
}
