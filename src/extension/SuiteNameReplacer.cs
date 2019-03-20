namespace NUnit.Engine.Listeners
{
    using System.IO;

    public class SuiteNameReplacer : ISuiteNameReplacer
    {
        private readonly ITeamCityInfo _teamCityInfo;

        public SuiteNameReplacer(ITeamCityInfo teamCityInfo)
        {
            _teamCityInfo = teamCityInfo;
        }


        public string Replace(string suiteName)
        {
            var assemblyFileName = Path.GetFileName(suiteName);
            if (string.IsNullOrEmpty(_teamCityInfo.SuitePattern))
            {
                return assemblyFileName;
            }

            return _teamCityInfo.SuitePattern
                .Replace("{n}", assemblyFileName)
                .Replace("{a}", Path.GetFileNameWithoutExtension(assemblyFileName));
        }
    }
}
