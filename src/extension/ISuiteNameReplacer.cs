namespace NUnit.Engine.Listeners
{
    internal interface ISuiteNameReplacer
    {
        string Replace(string suiteName);
    }
}