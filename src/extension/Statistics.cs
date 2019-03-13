namespace NUnit.Engine.Listeners
{
    internal class Statistics
    {
        private long _suiteStart;
        private long _suiteFinish;
        private long _testStart;
        private long _testFinish;

        public void RegisterSuiteStart()
        {
            _suiteStart++;
        }

        public void RegisterSuiteFinish()
        {
            _suiteFinish++;
        }

        public void RegisterTestStart()
        {
            _testStart++;
        }

        public void RegisterTestFinish()
        {
            _testFinish++;
        }

        public override string ToString()
        {
            return string.Format("SuiteStart: {0}, SuiteFinish: {1}, TestStart: {2}, TestFinish: {3}", _suiteStart, _suiteFinish, _testStart, _testFinish);
        }
    }
}
