namespace NUnit.Engine.Listeners
{
    using System;

    public class TeamCityInfo: ITeamCityInfo
    {
        // ReSharper disable once InconsistentNaming
        private static readonly TeamCityVersion TestMetadataSupportVersion = new TeamCityVersion("2018.2");
        private static readonly TeamCityVersion Version = new TeamCityVersion(Environment.GetEnvironmentVariable("TEAMCITY_VERSION"));
        private static readonly bool AllowExperimentalValue = GetBool(Environment.GetEnvironmentVariable("TEAMCITY_LOGGER_ALLOW_EXPERIMENTAL"), true);
        private static readonly int ProcessIdValue = System.Diagnostics.Process.GetCurrentProcess().Id;
        private static readonly bool MetadataEnabledValue =
            AllowExperimentalValue
            && GetBool(Environment.GetEnvironmentVariable("TEAMCITY_DOTNET_TEST_METADATA_ENABLE"), true)
            && Version.CompareTo(TestMetadataSupportVersion) >= 0;
        private static readonly string RootFlowIdValue = Environment.GetEnvironmentVariable("TEAMCITY_PROCESS_FLOW_ID") ?? "PID_" + ProcessIdValue;
        private static readonly bool AllowDiagnosticsValue = GetBool(Environment.GetEnvironmentVariable("TEAMCITY_NUNIT_DIAG"), false);
        private static readonly string SuitePatternValue = Environment.GetEnvironmentVariable("TEAMCITY_NUNIT_SUITE_PATTERN") ?? "";
        private static readonly string ColonReplacementValue = Environment.GetEnvironmentVariable("TEAMCITY_COLON_REPLACEMENT") ?? "<colon>";

        public bool MetadataEnabled
        {
            get { return MetadataEnabledValue; }
        }

        public string RootFlowId
        {
            get { return RootFlowIdValue; }
        }

        public bool AllowDiagnostics
        {
            get { return AllowDiagnosticsValue; }
        }

        public int ProcessId
        {
            get { return ProcessIdValue; }
        }

        public string SuitePattern
        {
            get { return SuitePatternValue; }
        }

        public string ColonReplacement
        {
            get { return ColonReplacementValue; }
        }

        private static bool GetBool(string value, bool defaultValue)
        {
            bool result;
            return !string.IsNullOrEmpty(value) && bool.TryParse(value, out result) ? result : defaultValue;
        }
    }
}
