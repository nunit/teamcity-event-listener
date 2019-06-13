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
    public class TeamCityEventListener2IntegrationTests
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
        [TestCase("Assembly")]
        [TestCase("SetUpFixture")]
        public void ShouldSendMessagesWithPredefinedRootFlowId(string finishSuiteName)
        {
            // Given
            var publisher = CreateInstance();

            // When
            publisher.RootFlowId = "root";
            publisher.RegisterMessage(TestUtil.CreateStartRun(1));

            // Assembly 1
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-1", null, "aaa" + Path.DirectorySeparatorChar + "Assembly1"));

            // Test Assembly1.Namespace1.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-2", null, "Assembly1.Namespace1.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("1-2", null, "Assembly1.Namespace1.1.Test1", "1.3", "Text output"));

            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-1", null, "Assembly1", finishSuiteName));

            publisher.RegisterMessage(TestUtil.CreateTestRun());

            // Then
            Assert.AreEqual(
               "##teamcity[testSuiteStarted name='Assembly1' flowId='root_1']" + Environment.NewLine

               + "##teamcity[testStarted name='Assembly1.Namespace1.1.Test1' captureStandardOutput='false' flowId='root_1']" + Environment.NewLine
               + "##teamcity[testStdOut name='Assembly1.Namespace1.1.Test1' out='Text output' flowId='root_1' tc:tags='tc:parseServiceMessagesInside']" + Environment.NewLine
               + "##teamcity[testFinished name='Assembly1.Namespace1.1.Test1' duration='1300' flowId='root_1']" + Environment.NewLine

               + "##teamcity[testSuiteFinished name='Assembly1' flowId='root_1']" + Environment.NewLine,
                _output.ToString());
        }

        [Test]
        [TestCase("Assembly")]
        [TestCase("SetUpFixture")]
        public void ShouldSendMessages(string finishSuiteName)
        {
            // Given
            var publisher = CreateInstance();

            // When
            publisher.RegisterMessage(TestUtil.CreateStartRun(1));

            // Assembly 1
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-1", null, "aaa" + Path.DirectorySeparatorChar + "Assembly1"));

            // Test Assembly1.Namespace1.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-2", null, "Assembly1.Namespace1.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("1-2", null, "Assembly1.Namespace1.1.Test1", "1.3", "Text output"));

            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-1", null, "Assembly1", finishSuiteName));

            publisher.RegisterMessage(TestUtil.CreateTestRun());

            // Then
            Assert.AreEqual(
        "##teamcity[testSuiteStarted name='Assembly1' flowId='1']" + Environment.NewLine

                + "##teamcity[testStarted name='Assembly1.Namespace1.1.Test1' captureStandardOutput='false' flowId='1']" + Environment.NewLine
                + "##teamcity[testStdOut name='Assembly1.Namespace1.1.Test1' out='Text output' flowId='1' tc:tags='tc:parseServiceMessagesInside']" + Environment.NewLine
                + "##teamcity[testFinished name='Assembly1.Namespace1.1.Test1' duration='1300' flowId='1']" + Environment.NewLine

                + "##teamcity[testSuiteFinished name='Assembly1' flowId='1']" + Environment.NewLine,
            _output.ToString());
        }

        [Test]
        [TestCase("Assembly")]
        [TestCase("SetUpFixture")]
        public void ShouldSendMessagesWhenParallelTests(string finishSuiteName)
        {
            // Given
            var publisher = CreateInstance();

            // When
            publisher.RegisterMessage(TestUtil.CreateStartRun(1));

            // Assembly 1
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-1", null, "aaa" + Path.DirectorySeparatorChar + "Assembly1"));
            // Assembly 2
            publisher.RegisterMessage(TestUtil.CreateStartSuite("2-1", null, "aaa" + Path.DirectorySeparatorChar + "Assembly2"));

            // Test Assembly2.Namespace1.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("2-2", null, "Assembly2.Namespace1.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("2-2", null, "Assembly2.Namespace1.1.Test1", "2.3", "Text output"));

            // Test Assembly1.Namespace1.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-2", null, "Assembly1.Namespace1.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("1-2", null, "Assembly1.Namespace1.1.Test1", "1.3", "Text output"));

            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-1", null, "Assembly1", finishSuiteName));
            publisher.RegisterMessage(TestUtil.CreateFinishSuite("2-1", null, "Assembly2", finishSuiteName));

            publisher.RegisterMessage(TestUtil.CreateTestRun());

            // Then
            Assert.AreEqual(
            "##teamcity[testSuiteStarted name='Assembly1' flowId='1']" + Environment.NewLine
                   + "##teamcity[testSuiteStarted name='Assembly2' flowId='2']" + Environment.NewLine

                   + "##teamcity[testStarted name='Assembly2.Namespace1.1.Test1' captureStandardOutput='false' flowId='2']" + Environment.NewLine
                   + "##teamcity[testStdOut name='Assembly2.Namespace1.1.Test1' out='Text output' flowId='2' tc:tags='tc:parseServiceMessagesInside']" + Environment.NewLine
                   + "##teamcity[testFinished name='Assembly2.Namespace1.1.Test1' duration='2300' flowId='2']" + Environment.NewLine

                   + "##teamcity[testStarted name='Assembly1.Namespace1.1.Test1' captureStandardOutput='false' flowId='1']" + Environment.NewLine
                   + "##teamcity[testStdOut name='Assembly1.Namespace1.1.Test1' out='Text output' flowId='1' tc:tags='tc:parseServiceMessagesInside']" + Environment.NewLine
                   + "##teamcity[testFinished name='Assembly1.Namespace1.1.Test1' duration='1300' flowId='1']" + Environment.NewLine

                   + "##teamcity[testSuiteFinished name='Assembly1' flowId='1']" + Environment.NewLine
                   + "##teamcity[testSuiteFinished name='Assembly2' flowId='2']" + Environment.NewLine,
            _output.ToString());
        }

        [Test]
        public void ShouldSendMessagesWithValidFlowIdWhenTests()
        {
            // Given
            var publisher = CreateInstance();

            // When
            publisher.RegisterMessage(TestUtil.CreateStartRun(1));

            // Assembly 1
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-1", null, "aaa" + Path.DirectorySeparatorChar + "Assembly1"));
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-2", null, "Assembly1.Namespace1"));
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-3", null, "Assembly1.Namespace1.1"));
            
            // Test Assembly1.Namespace1.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-4", null, "Assembly1.Namespace1.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("1-4", null, "Assembly1.Namespace1.1.Test1", "0.1", "Text output"));
            
            // Test Assembly1.Namespace1.1.Test2
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-5", null, "Assembly1.Namespace1.1.Test2"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseFailed("1-5", null, "Assembly1.Namespace1.1.Test2", "0.2", "Error output", "Stack trace"));
            
            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-3", null, "Assembly1.Namespace1.1", "namespace"));
            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-2", null, "Assembly1.Namespace1", "namespace"));
            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-1", null, "Assembly1", "Assembly"));

            // Assembly 2
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-6", null, "ddd//Assembly2"));
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-7", null, "Assembly2.Namespace2"));

            // Test Assembly2.Namespace2.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-8", null, "Assembly2.Namespace2.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("1-8", null, "Assembly2.Namespace2.1.Test1", "0.3", "Text output"));

            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-7", null, "Assembly2.Namespace2", "namespace"));
            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-6", null, "Assembly2", "Assembly"));

            publisher.RegisterMessage(TestUtil.CreateTestRun());

            // Then
            Assert.AreEqual(
                "##teamcity[testSuiteStarted name='Assembly1' flowId='1']" + Environment.NewLine

                + "##teamcity[testStarted name='Assembly1.Namespace1.1.Test1' captureStandardOutput='false' flowId='1']" + Environment.NewLine
                + "##teamcity[testStdOut name='Assembly1.Namespace1.1.Test1' out='Text output' flowId='1' tc:tags='tc:parseServiceMessagesInside']" + Environment.NewLine
                + "##teamcity[testFinished name='Assembly1.Namespace1.1.Test1' duration='100' flowId='1']" + Environment.NewLine

                + "##teamcity[testStarted name='Assembly1.Namespace1.1.Test2' captureStandardOutput='false' flowId='1']" + Environment.NewLine
                + "##teamcity[testFailed name='Assembly1.Namespace1.1.Test2' message='Error output' details='Stack trace' flowId='1']" + Environment.NewLine
                + "##teamcity[testFinished name='Assembly1.Namespace1.1.Test2' duration='200' flowId='1']" + Environment.NewLine

                + "##teamcity[testSuiteFinished name='Assembly1' flowId='1']" + Environment.NewLine
                
                + "##teamcity[testSuiteStarted name='Assembly2' flowId='1']" + Environment.NewLine
                + "##teamcity[testStarted name='Assembly2.Namespace2.1.Test1' captureStandardOutput='false' flowId='1']" + Environment.NewLine
                + "##teamcity[testStdOut name='Assembly2.Namespace2.1.Test1' out='Text output' flowId='1' tc:tags='tc:parseServiceMessagesInside']" + Environment.NewLine
                + "##teamcity[testFinished name='Assembly2.Namespace2.1.Test1' duration='300' flowId='1']" + Environment.NewLine

                + "##teamcity[testSuiteFinished name='Assembly2' flowId='1']" + Environment.NewLine,
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
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-1", null, "aaa" + Path.DirectorySeparatorChar + "Assembly1"));

            // Test Assembly1.Namespace1.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-2", null, "Assembly1.Namespace1.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("1-2", null, "Assembly1.Namespace1.1.Test1", "1.3", "Text output"));

            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-1", null, "Assembly1", "Assembly"));

            publisher.RegisterMessage(TestUtil.CreateTestRun());

            // Then
            Assert.AreEqual(
                "##teamcity[testSuiteStarted name='Assembly1' flowId='1']" + Environment.NewLine

                + "##teamcity[testStarted name='Assembly1.Namespace1.1.Test1' captureStandardOutput='false' flowId='1']" + Environment.NewLine
                + "##teamcity[testStdOut name='Assembly1.Namespace1.1.Test1' out='Text output' flowId='1' tc:tags='tc:parseServiceMessagesInside']" + Environment.NewLine
                + "##teamcity[testFinished name='Assembly1.Namespace1.1.Test1' duration='1300' flowId='1']" + Environment.NewLine
                
                + "##teamcity[testSuiteFinished name='Assembly1' flowId='1']" + Environment.NewLine,
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
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-1", null, "Assembly1.Namespace1.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseSuccessful("1-1", null, "Assembly1.Namespace1.1.Test1", "0.1", "Text output"));

            publisher.RegisterMessage(TestUtil.CreateTestRun());

            // Then
            Assert.AreEqual(
                "##teamcity[testStarted name='Assembly1.Namespace1.1.Test1' captureStandardOutput='false' flowId='1']" + Environment.NewLine
                + "##teamcity[testStdOut name='Assembly1.Namespace1.1.Test1' out='Text output' flowId='1' tc:tags='tc:parseServiceMessagesInside']" + Environment.NewLine
                + "##teamcity[testFinished name='Assembly1.Namespace1.1.Test1' duration='100' flowId='1']" + Environment.NewLine,
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
            publisher.RegisterMessage(TestUtil.CreateStartSuite("1-1", null, "aaa" + Path.DirectorySeparatorChar + "Assembly1"));

            // Test Assembly1.Namespace1.1.Test1
            publisher.RegisterMessage(TestUtil.CreateStartTest("1-1", null, "Assembly1.Namespace1.1.Test1"));
            publisher.RegisterMessage(TestUtil.CreateTestCaseFailed("1-1", null, "Assembly1.Namespace1.1.Test1", "0.1", "TestFixtureSetUp failed in Test1", "Stack trace xyz"));

            publisher.RegisterMessage(TestUtil.CreateFinishSuite("1-1", null, "Assembly1", "Assembly"));

            publisher.RegisterMessage(TestUtil.CreateTestRun());

            // Then
            Assert.AreEqual(
                "##teamcity[testSuiteStarted name='Assembly1' flowId='1']" + Environment.NewLine

                + "##teamcity[testStarted name='Assembly1.Namespace1.1.Test1' captureStandardOutput='false' flowId='1']" + Environment.NewLine
                + "##teamcity[testFailed name='Assembly1.Namespace1.1.Test1' message='One or more child tests had errors' details='stack abc' flowId='1']" + Environment.NewLine
                + "##teamcity[testFinished name='Assembly1.Namespace1.1.Test1' duration='100' flowId='1']" + Environment.NewLine
                
                + "##teamcity[testSuiteFinished name='Assembly1' flowId='1']" + Environment.NewLine,
                _output.ToString());
        }        

        private TeamCityEventListener CreateInstance()
        {
            return new TeamCityEventListener(_outputWriter, new TeamCityInfo()) { RootFlowId = string.Empty };
        }
    }
}
