﻿namespace NUnit.Engine.Listeners
{
    using System;

    public static class TeamCityInfo
    {
        private static readonly TeamCityVersion TestMetadataSupportVersion = new TeamCityVersion("2018.2");
        private static readonly TeamCityVersion Version = new TeamCityVersion(Environment.GetEnvironmentVariable("TEAMCITY_VERSION"));
        private static readonly bool AllowExperimental = GetBool(Environment.GetEnvironmentVariable("TEAMCITY_LOGGER_ALLOW_EXPERIMENTAL"), true);
        public static readonly bool MetadataEnabled =
            TeamCityInfo.AllowExperimental
            && GetBool(Environment.GetEnvironmentVariable("TEAMCITY_DOTNET_TEST_METADATA_ENABLE"), true)
            && TeamCityInfo.Version.CompareTo(TeamCityInfo.TestMetadataSupportVersion) >= 0;
        public static readonly string RootFlowId = Environment.GetEnvironmentVariable("TEAMCITY_PROCESS_FLOW_ID");

        private static bool GetBool(string value, bool defaultValue)
        {
            bool result;
            return !string.IsNullOrEmpty(value) && bool.TryParse(value, out result) ? result : defaultValue;
        }
    }
}