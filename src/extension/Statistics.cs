﻿// ***********************************************************************
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
    public class Statistics
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
