namespace nunit.integration.tests.Templates
{
    using System;

    using NUnit.Framework;

    /// <summary>
    /// Used as template for test methods.
    /// The name of the methods should be equal to the TestType members.
    /// </summary>
    [TestFixture]
    internal class UnitTest
    {
        public void ThrowException()
        {
            throw new System.Exception("Exception");
        }

        [SetUp]
        public void FailedSeTup()
        {
            throw new System.Exception("Exception during setup");
        }

        [OneTimeSetUp]
        public void FailedOneTimeSetUp()
        {
            throw new System.Exception("Exception during one time setup");
        }        

        [TearDown]
        public void FailedTearDown()
        {
            throw new System.Exception("Exception during tear down");
        }

        [SetUp]
        public void SetUpWithOutput()
        {
            System.Console.WriteLine("SetUp output");
        }

        [TearDown]
        public void TearDownWithOutput()
        {
            System.Console.WriteLine("TearDown output");
        }

        [Test]
        public void Successful()
        {
            System.Console.WriteLine("output");
        }

        [Test]
        public void Pass()
        {
            Assert.Pass();
        }

        [Test]
        public void PassWithText()
        {
            Assert.Pass("some text");
        }

        [Test]
        public void Failed()
        {
            Assert.Fail("Reason");
        }

        [Test]
        public void Ignored()
        {
            Assert.Ignore("Reason");
        }

        [Test]
        public void Inconclusive()
        {
            Assert.Inconclusive("Reason");
        }

        [Test, Category("CatA")]
        public void SuccessfulCatA()
        {
        }

        [Test, Parallelizable]
        public void SuccessfulParallelizable()
        {
            System.Console.WriteLine($"!!! ManagedThreadId = {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            System.Threading.Thread.Sleep(100);
        }

        [Test]
        public void FailedStackOverflow()
        {
            System.Action[] infiniteRecursion = null;
            infiniteRecursion = new System.Action[1] { () => { infiniteRecursion[0](); } };
            infiniteRecursion[0]();
        }

        [Test]
        public void FailedOutOfMemory()
        {
            System.Collections.Generic.List<byte[]> bytes = new System.Collections.Generic.List<byte[]>();
            while (true)
            {
                bytes.Add(new byte[0xffffff]);
            }
        }

        [Test]
        public void SuccessfulWithConfig()
        {
            System.Console.Write(System.IO.Path.GetFileName(System.AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            var message = System.Configuration.ConfigurationManager.AppSettings["TestMessage"];
            Assert.IsNotNull(message);
        }

        [Test]
        public void UnloadingDomain()
        {
            UnloadingDomainUtil.Create();
        }
    }
}