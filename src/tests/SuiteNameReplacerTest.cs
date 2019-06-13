namespace NUnit.Engine.Listeners
{
    using Framework;

    [TestFixture]
    public class SuiteNameReplacerTest
    {
        [Test]
        [TestCase("abc.dll", "abc.dll")]
        [TestCase("dir\\abc.dll", "abc.dll")]
        [TestCase("c:\\dir\\abc.dll", "abc.dll")]
        [TestCase(" ", " ")]
        [TestCase(null, null)]
        public void ShouldExtractAssemblyFileNameFromPath(string name, string expectedSuiteName)
        {
            // Given
            var suiteNameReplacer = new SuiteNameReplacer(new MyTeamCityInfo());

            // When
            var actualSuiteName = suiteNameReplacer.Replace(name);

            // Then        
            Assert.AreEqual(expectedSuiteName, actualSuiteName);
        }

        [Test]
        [TestCase("abc.dll", "", "abc.dll")]
        [TestCase("abc.dll", "x64_{n}", "x64_abc.dll")]
        [TestCase("c:\\dir\\abc.dll", "x64_{n}", "x64_abc.dll")]
        [TestCase("c:\\dir\\abc.dll", "x64_{n}_xyz", "x64_abc.dll_xyz")]
        [TestCase("c:\\dir\\abc.dll", "{n}_xyz", "abc.dll_xyz")]
        public void ShouldReplaceName(string name, string suitePattern, string expectedSuiteName)
        {
            // Given
            var suiteNameReplacer = new SuiteNameReplacer(new MyTeamCityInfo { SuitePattern = suitePattern });

            // When
            var actualSuiteName = suiteNameReplacer.Replace(name);

            // Then        
            Assert.AreEqual(expectedSuiteName, actualSuiteName);
        }

        [Test]
        [TestCase("abc.dll", "x64_{a}", "x64_abc")]
        [TestCase("c:\\dir\\abc.dll", "x64_{a}", "x64_abc")]
        [TestCase("c:\\dir\\abc.dll", "x64_{a}_xyz", "x64_abc_xyz")]
        [TestCase("c:\\dir\\abc.dll", "{a}_xyz", "abc_xyz")]
        public void ShouldReplaceAssemblyName(string name, string suitePattern, string expectedSuiteName)
        {
            // Given
            var suiteNameReplacer = new SuiteNameReplacer(new MyTeamCityInfo { SuitePattern = suitePattern });

            // When
            var actualSuiteName = suiteNameReplacer.Replace(name);

            // Then        
            Assert.AreEqual(expectedSuiteName, actualSuiteName);
        }

        private class MyTeamCityInfo : ITeamCityInfo
        {
            public bool MetadataEnabled { get; set; }

            public string RootFlowId { get; set; }

            public bool AllowDiagnostics { get; set; }

            public int ProcessId { get; set; }

            public string SuitePattern { get; set; }

            public string ColonReplacement { get; set; }
        }
    }
}
