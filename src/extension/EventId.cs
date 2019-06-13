namespace NUnit.Engine.Listeners
{
    internal struct EventId
    {
        public readonly string FlowId;
        public readonly string FullName;

        public EventId(ITeamCityInfo tamCityInfo, string flowId, string fullName)
        {
            FlowId = flowId;
            // TeamCity extracts the name of assembly from the test name
            // For instance:
            //
            // abc.dll: text1:text2
            // assembly name = "abc.dll: text1"
            // test name = "text2"
            //
            // or
            //
            // abc.dll: text1
            // assembly name = "abc.dll"
            // test name = "text1"

            FullName = fullName.Replace(":", tamCityInfo.ColonReplacement);
        }
    }
}
