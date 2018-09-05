namespace NUnit.Engine.Listeners
{
    internal struct EventId
    {
        public readonly string FlowId;
        public readonly string FullName;

        public EventId(string flowId, string fullName)
        {
            FlowId = flowId;
            FullName = fullName;
        }
    }
}
