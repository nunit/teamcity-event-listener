namespace NUnit.Engine.Listeners
{
    internal interface ITeamCityInfo
    {
        bool MetadataEnabled { get; }

        string RootFlowId { get; }

        bool AllowDiagnostics { get; }

        int ProcessId { get; }
    }
}
