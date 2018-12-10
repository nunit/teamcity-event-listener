namespace NUnit.Engine.Listeners
{
    using Framework;

    [TestFixture]
    public class TeamCityVersionTest
    {
        [Test]
        [TestCase("2018.1", "2018.2", -1)]
        [TestCase("2018.1", "2018.2 (build SNAPSHOT)", -1)]
        [TestCase("2018.2", "2018.2 (build SNAPSHOT)", 0)]
        [TestCase("2018.2 (build SNAPSHOT)", "2018.2 (build SNAPSHOT)", 0)]
        [TestCase("2018.2 (build SNAPSHOT)", "2018.2 (build SNAPSHOT 2)", 0)]
        [TestCase("2018.2 (build SNAPSHOT)", "2018.1", 1)]
        [TestCase("", "2018.2 (build SNAPSHOT)", -1)]
        [TestCase("2018.2 (build SNAPSHOT)", "", 1)]
        [TestCase("2018.2", "2018.2", 0)]
        [TestCase("2018.2.1", "2018.2", 0)]
        [TestCase(null, "2018.2", -1)]
        [TestCase("", "", 0)]
        [TestCase(null, "", 0)]
        [TestCase(null, null, 0)]
        [TestCase("", "abc", 0)]
        [TestCase(".32323", "abc", 0)]
        [TestCase("2018", "2018", 0)]
        [TestCase("2018", "2018.1", -1)]
        [TestCase("10", "11", -1)]
        [TestCase("12", "11", 1)]
        public void ShouldCompareTeamCityVersions(string version1, string version2, int expectedCompareResult)
        {
            // Given
            var v1 = new TeamCityVersion(version1);
            var v2 = new TeamCityVersion(version2);

            // When
            var actualCompareResult = v1.CompareTo(v2);

            // Then
            Assert.AreEqual(expectedCompareResult, actualCompareResult);
        }
    }
}
