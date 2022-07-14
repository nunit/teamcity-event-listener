// ***********************************************************************
// Copyright (c) 2015 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

namespace NUnit.Engine.Listeners
{
    using System;
    using System.Collections.Generic;

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
        // Semicolon separated list of TeamCity message names (testStdOut;testStdErr;testFailed;testIgnored...) to suppress parsing TeamCity messages inside a parent message
        private static readonly string MessagesToSuppressParsingMessagesInsideValue = Environment.GetEnvironmentVariable("TEAMCITY_SUPPRESS_PARSING_INSIDE") ?? "";
        private static readonly Dictionary<string, string> MessagesToSuppressParsingMessagesInside = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        static TeamCityInfo()
        {
            foreach (var eventId in MessagesToSuppressParsingMessagesInsideValue.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                MessagesToSuppressParsingMessagesInside[eventId] = string.Empty;
            }
        }

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

        public bool SuppressParsingMessagesInside(string eventId)
        {
            return MessagesToSuppressParsingMessagesInside.ContainsKey(eventId);
        }

        private static bool GetBool(string value, bool defaultValue)
        {
            bool result;
            return !string.IsNullOrEmpty(value) && bool.TryParse(value, out result) ? result : defaultValue;
        }
    }
}
