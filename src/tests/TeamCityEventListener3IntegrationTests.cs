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
    using System.IO;
    using System.Text;
    using Extensibility;
    using Framework;

    [TestFixture]
    public class TeamCityEventListener3IntegrationTests
    {
        private StringBuilder _output;
        private StringWriter _outputWriter;

        [SetUp]
        public void SetUp()
        {
            _output = new StringBuilder();
            _outputWriter = new StringWriter(_output);
        }

        [TearDown]
        public void TearDown()
        {
            _outputWriter.Dispose();
        }

        [Test]
        public void CheckExtensionAttribute()
        {
            Assert.That(typeof(TeamCityEventListener),
                Has.Attribute<ExtensionAttribute>()
                    .With.Property("EngineVersion").EqualTo("3.4")
                    .And.Property("Enabled").EqualTo(false));
        }

        [Test]
        public void ShouldSendMessagesWithValidFlowIdWhenParallelizedTests()
        {
            // Given
            var publisher = CreateInstance();

            // When
            publisher.RegisterMessage(TestUtil.CreateStartRun(1));

            // Assembly 1
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-1", "", "aaa" + Path.DirectorySeparatorChar + "Assembly1"));
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-2", "1-1", "Assembly1.Namespace1"));
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-3", "1-2", "Assembly1.Namespace1.1"));

            // Assembly 2
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-6", "", "ddd" + Path.DirectorySeparatorChar + "Assembly2"));
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-7", "1-6", "Assembly2.Namespace2"));

            // Test Assembly1.Namespace1.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-4", "1-3", "Assembly1.Namespace1.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("1-4", "1-3", "Assembly1.Namespace1.1.Test1", "0.1", "Text output"));

            // Test Assembly2.Namespace2.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-8", "1-7", "Assembly2.Namespace2.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("1-8", "1-7", "Assembly2.Namespace2.1.Test1", "0.1", "Text output"));

            // Test Assembly1.Namespace1.1.Test2
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-5", "1-3", "Assembly1.Namespace1.1.Test2"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseFailed("1-5", "1-3", "Assembly1.Namespace1.1.Test2", "0.2", "Error output", "Stack trace"));

            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-7", "1-6", "Assembly2.Namespace2", ""));
            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-6", "", "Assembly2", ""));

            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-3", "1-2",  "Assembly1.Namespace1.1", ""));
            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-2", "1-1", "Assembly1.Namespace1", ""));
            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-1", "", "Assembly1", ""));

            publisher.RegisterMessage(TestUtil.CreateTestRun());

            // Then
            Assert.AreEqual(
                "##teamcity[flowStarted flowId='1-1' parent='.']" + Environment.NewLine
                + "##teamcity[testSuiteStarted name='Assembly1' flowId='1-1']" + Environment.NewLine
                + "##teamcity[flowStarted flowId='1-6' parent='.']" + Environment.NewLine
                + "##teamcity[testSuiteStarted name='Assembly2' flowId='1-6']" + Environment.NewLine
                
                + "##teamcity[flowStarted flowId='1-4' parent='1-1']" + Environment.NewLine
                + "##teamcity[testStarted name='Assembly1.Namespace1.1.Test1' captureStandardOutput='false' flowId='1-4']" + Environment.NewLine
                + "##teamcity[testStdOut name='Assembly1.Namespace1.1.Test1' out='Text output' flowId='1-4' tc:tags='tc:parseServiceMessagesInside']" + Environment.NewLine
                + "##teamcity[testFinished name='Assembly1.Namespace1.1.Test1' duration='100' flowId='1-4']" + Environment.NewLine
                + "##teamcity[flowFinished flowId='1-4']" + Environment.NewLine

                + "##teamcity[flowStarted flowId='1-8' parent='1-6']" + Environment.NewLine
                + "##teamcity[testStarted name='Assembly2.Namespace2.1.Test1' captureStandardOutput='false' flowId='1-8']" + Environment.NewLine
                + "##teamcity[testStdOut name='Assembly2.Namespace2.1.Test1' out='Text output' flowId='1-8' tc:tags='tc:parseServiceMessagesInside']" + Environment.NewLine
                + "##teamcity[testFinished name='Assembly2.Namespace2.1.Test1' duration='100' flowId='1-8']" + Environment.NewLine
                + "##teamcity[flowFinished flowId='1-8']" + Environment.NewLine

                + "##teamcity[flowStarted flowId='1-5' parent='1-1']" + Environment.NewLine
                + "##teamcity[testStarted name='Assembly1.Namespace1.1.Test2' captureStandardOutput='false' flowId='1-5']" + Environment.NewLine
                + "##teamcity[testFailed name='Assembly1.Namespace1.1.Test2' message='Error output' details='Stack trace' flowId='1-5']" + Environment.NewLine
                + "##teamcity[testFinished name='Assembly1.Namespace1.1.Test2' duration='200' flowId='1-5']" + Environment.NewLine
                + "##teamcity[flowFinished flowId='1-5']" + Environment.NewLine

                + "##teamcity[testSuiteFinished name='Assembly2' flowId='1-6']" + Environment.NewLine
                + "##teamcity[flowFinished flowId='1-6']" + Environment.NewLine
                + "##teamcity[testSuiteFinished name='Assembly1' flowId='1-1']" + Environment.NewLine
                + "##teamcity[flowFinished flowId='1-1']" + Environment.NewLine,
                _output.ToString());
        }

        [Test]
        public void ShouldSendMessagesWithValidFlowIdWhenHas1Suite()
        {
            // Given
            var publisher = CreateInstance();

            // When
            publisher.RegisterMessage(TestUtil.CreateStartRun(1));

            // Assembly 1
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-1", "", "aaa" + Path.DirectorySeparatorChar + "Assembly1"));
            
            // Test Assembly1.Namespace1.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-2", "1-1", "Assembly1.Namespace1.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("1-2", "1-1", "Assembly1.Namespace1.1.Test1", "0.1", "Text output"));

            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-1", "", "Assembly1", ""));

            publisher.RegisterMessage(TestUtil.CreateTestRun());

            // Then
            Assert.AreEqual(
                "##teamcity[flowStarted flowId='1-1' parent='.']" + Environment.NewLine
                + "##teamcity[testSuiteStarted name='Assembly1' flowId='1-1']" + Environment.NewLine

                + "##teamcity[flowStarted flowId='1-2' parent='1-1']" + Environment.NewLine
                + "##teamcity[testStarted name='Assembly1.Namespace1.1.Test1' captureStandardOutput='false' flowId='1-2']" + Environment.NewLine
                + "##teamcity[testStdOut name='Assembly1.Namespace1.1.Test1' out='Text output' flowId='1-2' tc:tags='tc:parseServiceMessagesInside']" + Environment.NewLine
                + "##teamcity[testFinished name='Assembly1.Namespace1.1.Test1' duration='100' flowId='1-2']" + Environment.NewLine
                + "##teamcity[flowFinished flowId='1-2']" + Environment.NewLine

                + "##teamcity[testSuiteFinished name='Assembly1' flowId='1-1']" + Environment.NewLine
                + "##teamcity[flowFinished flowId='1-1']" + Environment.NewLine,
                _output.ToString());
        }

        [Test]
        [TestCase("abc", "abc")]
        [TestCase(".", ".")]
        public void ShouldSendMessagesWithValidFlowIdWhenNestedSuites(string rootFlowId, string expectedId)
        {
            // Given
            var publisher = CreateInstance();
            publisher.RootFlowId = rootFlowId;

            // When
            publisher.RegisterMessage(TestUtil.CreateStartRun(1));

            // Suite 1
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1", "", "aaa" + Path.DirectorySeparatorChar + "Suite1"));

            // Suite 2
            publisher.RegisterMessage(TestUtil.CreateStartSuite("2", "1", "Suite1.Suite2"));

            // Suite 3
            publisher.RegisterMessage(TestUtil.CreateStartSuite("3", "2", "Suite1.Suite2.Suite3"));

            // Suite 4
            publisher.RegisterMessage(TestUtil.CreateStartSuite("4", "2", "Suite1.Suite2.Suite4"));

            // Test Suite1.Suite2.Suite3.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("5", "3", "Suite1.Suite2.Suite3.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("5", "3", "Suite1.Suite2.Suite3.1.Test1", "0.1", "Text output"));

            // Test Suite1.Suite2.Suite4.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("6", "4", "Suite1.Suite2.Suite4.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("6", "4", "Suite1.Suite2.Suite4.1.Test1", "0.1", "Text output"));

            publisher.RegisterMessage(TestUtil.CreateFinishSuite("3", "2", "Suite1.Suite2.Suite3", ""));
            publisher.RegisterMessage(TestUtil.CreateFinishSuite("4", "2", "Suite1.Suite2.Suite4", ""));
            publisher.RegisterMessage(TestUtil.CreateFinishSuite("2", "1", "Suite1.Suite2", ""));
            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1", "", "Suite1", ""));

            publisher.RegisterMessage(TestUtil.CreateTestRun());

            // Then
            Assert.AreEqual(
            "##teamcity[flowStarted flowId='" + rootFlowId + "_1' parent='" + expectedId + "']" + Environment.NewLine
                  + "##teamcity[testSuiteStarted name='Suite1' flowId='" + rootFlowId + "_1']" + Environment.NewLine

                  + "##teamcity[flowStarted flowId='" + rootFlowId + "_5' parent='" + rootFlowId + "_1']" + Environment.NewLine

                  + "##teamcity[testStarted name='Suite1.Suite2.Suite3.1.Test1' captureStandardOutput='false' flowId='" + rootFlowId + "_5']" + Environment.NewLine
                  + "##teamcity[testStdOut name='Suite1.Suite2.Suite3.1.Test1' out='Text output' flowId='" + rootFlowId + "_5' tc:tags='tc:parseServiceMessagesInside']" + Environment.NewLine
                  + "##teamcity[testFinished name='Suite1.Suite2.Suite3.1.Test1' duration='100' flowId='" + rootFlowId + "_5']" + Environment.NewLine

                  + "##teamcity[flowFinished flowId='" + rootFlowId + "_5']" + Environment.NewLine

                  + "##teamcity[flowStarted flowId='" + rootFlowId + "_6' parent='" + rootFlowId + "_1']" + Environment.NewLine

                  + "##teamcity[testStarted name='Suite1.Suite2.Suite4.1.Test1' captureStandardOutput='false' flowId='" + rootFlowId + "_6']" + Environment.NewLine
                  + "##teamcity[testStdOut name='Suite1.Suite2.Suite4.1.Test1' out='Text output' flowId='" + rootFlowId + "_6' tc:tags='tc:parseServiceMessagesInside']" + Environment.NewLine
                  + "##teamcity[testFinished name='Suite1.Suite2.Suite4.1.Test1' duration='100' flowId='" + rootFlowId + "_6']" + Environment.NewLine
                  
                  + "##teamcity[flowFinished flowId='" + rootFlowId + "_6']" + Environment.NewLine

                  + "##teamcity[testSuiteFinished name='Suite1' flowId='" + rootFlowId + "_1']" + Environment.NewLine
                  + "##teamcity[flowFinished flowId='" + rootFlowId + "_1']" + Environment.NewLine,
                _output.ToString());
        }

        [Test]
        public void ShouldSendMessagesWithPredefinedRootFlowId()
        {
            // Given
            var publisher = CreateInstance();

            // When
            publisher.RootFlowId = "root";
            publisher.RegisterMessage(TestUtil.CreateStartRun(1));

            // Assembly 1
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-1", "", "aaa" + Path.DirectorySeparatorChar + "Assembly1"));
            
            // Test Assembly1.Namespace1.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-2", "1-1", "Assembly1.Namespace1.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("1-2", "1-1", "Assembly1.Namespace1.1.Test1", "0.1", "Text output"));

            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-1", "", "Assembly1", ""));

            publisher.RegisterMessage(TestUtil.CreateTestRun());

            // Then
            Assert.AreEqual(
                "##teamcity[flowStarted flowId='root_1-1' parent='root']" + Environment.NewLine
                + "##teamcity[testSuiteStarted name='Assembly1' flowId='root_1-1']" + Environment.NewLine

                + "##teamcity[flowStarted flowId='root_1-2' parent='root_1-1']" + Environment.NewLine
                + "##teamcity[testStarted name='Assembly1.Namespace1.1.Test1' captureStandardOutput='false' flowId='root_1-2']" + Environment.NewLine
                + "##teamcity[testStdOut name='Assembly1.Namespace1.1.Test1' out='Text output' flowId='root_1-2' tc:tags='tc:parseServiceMessagesInside']" + Environment.NewLine
                + "##teamcity[testFinished name='Assembly1.Namespace1.1.Test1' duration='100' flowId='root_1-2']" + Environment.NewLine
                + "##teamcity[flowFinished flowId='root_1-2']" + Environment.NewLine

                + "##teamcity[testSuiteFinished name='Assembly1' flowId='root_1-1']" + Environment.NewLine
                + "##teamcity[flowFinished flowId='root_1-1']" + Environment.NewLine,
                _output.ToString());
        }

        [Test]
        public void ShouldSendMessagesWithValidFlowIdWhenHasNoSuite()
        {
            // Given
            var publisher = CreateInstance();

            // When
            publisher.RegisterMessage(TestUtil.CreateStartRun(1));

            // Test Assembly1.Namespace1.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-1", "", "Assembly1.Namespace1.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("1-1", "", "Assembly1.Namespace1.1.Test1", "0.1", "Text output"));

            publisher.RegisterMessage(TestUtil.CreateTestRun());

            // Then
            Assert.AreEqual(
                "##teamcity[testStarted name='Assembly1.Namespace1.1.Test1' captureStandardOutput='false' flowId='1-1']" + Environment.NewLine
                + "##teamcity[testStdOut name='Assembly1.Namespace1.1.Test1' out='Text output' flowId='1-1' tc:tags='tc:parseServiceMessagesInside']" + Environment.NewLine
                + "##teamcity[testFinished name='Assembly1.Namespace1.1.Test1' duration='100' flowId='1-1']" + Environment.NewLine,
                _output.ToString());
        }

        [Test]
        public void ShouldSendMessagesWithValidFlowIdWhenOutputIsEmpty()
        {
            // Given
            var publisher = CreateInstance();

            // When
            publisher.RegisterMessage(TestUtil.CreateStartRun(1));

            // Test Assembly1.Namespace1.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-1", "", "Assembly1.Namespace1.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("1-1", "", "Assembly1.Namespace1.1.Test1", "0.1", ""));

            publisher.RegisterMessage(TestUtil.CreateTestRun());

            // Then
            Assert.AreEqual(
                "##teamcity[testStarted name='Assembly1.Namespace1.1.Test1' captureStandardOutput='false' flowId='1-1']" + Environment.NewLine
                + "##teamcity[testFinished name='Assembly1.Namespace1.1.Test1' duration='100' flowId='1-1']" + Environment.NewLine,
                _output.ToString());
        }

        [Test]
        public void ShouldSendMessagesWithValidFlowIdWhenHasNoOutput()
        {
            // Given
            var publisher = CreateInstance();

            // When
            publisher.RegisterMessage(TestUtil.CreateStartRun(1));

            // Test Assembly1.Namespace1.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-1", "", "Assembly1.Namespace1.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("1-1", "", "Assembly1.Namespace1.1.Test1", "10", null));

            publisher.RegisterMessage(TestUtil.CreateTestRun());

            // Then
            Assert.AreEqual(
                "##teamcity[testStarted name='Assembly1.Namespace1.1.Test1' captureStandardOutput='false' flowId='1-1']" + Environment.NewLine
                + "##teamcity[testFinished name='Assembly1.Namespace1.1.Test1' duration='10000' flowId='1-1']" + Environment.NewLine,
                _output.ToString());
        }        

        [Test]
        public void ShouldSendMessagesWhenOneTimeSetUpFailed()
        {
            // Given
            var publisher = CreateInstance();

            // When
            publisher.RegisterMessage(TestUtil.CreateStartRun(1));

            // Assembly 1
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-1", "", "aaa" + Path.DirectorySeparatorChar + "Assembly1"));

            // Test Assembly1.Namespace1.1.Test1
            publisher.RegisterMessage(TestUtil.CreateTestCaseFailed("1-2", "1-1", "Assembly1.Namespace1.1.Test1", "0.1", "Error output xyz", "Stack trace xyz"));

            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-1", "", "Assembly1", ""));

            publisher.RegisterMessage(TestUtil.CreateTestRun());

            // Then
            Assert.AreEqual(
                "##teamcity[flowStarted flowId='1-1' parent='.']" + Environment.NewLine
                + "##teamcity[testSuiteStarted name='Assembly1' flowId='1-1']" + Environment.NewLine

                + "##teamcity[testStarted name='Assembly1.Namespace1.1.Test1' captureStandardOutput='false' flowId='1-1']" + Environment.NewLine
                + "##teamcity[testFailed name='Assembly1.Namespace1.1.Test1' message='One or more child tests had errors' details='stack abc' flowId='1-1']" + Environment.NewLine
                + "##teamcity[testFinished name='Assembly1.Namespace1.1.Test1' duration='100' flowId='1-1']" + Environment.NewLine

                + "##teamcity[testSuiteFinished name='Assembly1' flowId='1-1']" + Environment.NewLine
                + "##teamcity[flowFinished flowId='1-1']" + Environment.NewLine,
                _output.ToString());
        }


        [Test]
        [TestCase("", ":", "")]
        [TestCase("abc", ":", "abc")]
        public void ShouldReplaceWhenColon(string colonReplacement, string replacingStr, string replacedStr)
        {
            // Given
            var publisher = CreateInstance(new MyTeamCityInfo { ColonReplacement  = colonReplacement });

            // When
            publisher.RegisterMessage(TestUtil.CreateStartRun(1));

            // Test Assembly1.Namespace1.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-1", "", "Assembly1.Namespace1.1.Test1(&quot;" + replacingStr + "&quot;)"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("1-1", "", "Assembly1.Namespace1.1.Test1(&quot;" + replacingStr + "&quot;)", "10", null));

            publisher.RegisterMessage(TestUtil.CreateTestRun());

            // Then
            Assert.AreEqual(
                "##teamcity[testStarted name='Assembly1.Namespace1.1.Test1(\"" + replacedStr + "\")' captureStandardOutput='false' flowId='1-1']"
                + Environment.NewLine
                + "##teamcity[testFinished name='Assembly1.Namespace1.1.Test1(\"" + replacedStr + "\")' duration='10000' flowId='1-1']" + Environment.NewLine,
                _output.ToString());
        }

        private TeamCityEventListener CreateInstance(ITeamCityInfo teamCityInfo = null)
        {
            return new TeamCityEventListener(_outputWriter, teamCityInfo != null ? teamCityInfo : new TeamCityInfo()) { RootFlowId = string.Empty };
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
