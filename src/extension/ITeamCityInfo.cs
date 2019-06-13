namespace NUnit.Engine.Listeners
{
    public interface ITeamCityInfo
    {
        bool MetadataEnabled { get; }

        string RootFlowId { get; }

        bool AllowDiagnostics { get; }

        int ProcessId { get; }

        string SuitePattern { get; }

        string ColonReplacement { get; }
    }
}
