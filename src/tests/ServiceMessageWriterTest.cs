namespace NUnit.Engine.Listeners
{
    using Framework;

    [TestFixture]
    public class ServiceMessageWriterTest
    {
        // Simple
        [TestCase("", "")]
        [TestCase("!@#$%^&*", "!@#$%^&*")]
        [TestCase("a", "a")]
        [TestCase("Abc", "Abc")]

        // Special
        [TestCase("|n", "\n")]
        [TestCase("|r", "\r")]
        [TestCase("|]", "]")]
        [TestCase("|[", "[")]
        [TestCase("|'", "'")]
        [TestCase("||", "|")]
        [TestCase("|x", "\u0085")]
        [TestCase("|l", "\u2028")]
        [TestCase("|p", "\u2029")]

        [TestCase("aaa|nbbb", "aaa\nbbb")]
        [TestCase("aaa|nbbb||", "aaa\nbbb|")]
        [TestCase("||||", "||")]

        // Unicode
        [TestCase("|0x00bf", "\u00bf")]
        [TestCase("|0x00bfaaa", "\u00bfaaa")]
        [TestCase("bb|0x00bfaaa", "bb\u00bfaaa")]
        public void ShouldEscapeString(string textFormServiceMessage, string actualText)
        {
            Assert.AreEqual(textFormServiceMessage, ServiceMessageWriter.EscapeString(actualText));
        }
    }
}
