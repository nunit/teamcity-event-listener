// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:3.0.0.0
//      SpecFlow Generator Version:3.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace nunit.integration.tests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("User runs tests")]
    public partial class UserRunsTestsFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "RunTests.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "User runs tests", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 3
#line 4
    testRunner.Given("NUnit path is ..\\nunit\\", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User runs tests for several assemblies")]
        [NUnit.Framework.CategoryAttribute("3.4.1")]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Single", "2", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Single", "2", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Single", "2", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Multiple", "Single", "2", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Multiple", "Single", "2", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "InProcess", "Multiple", "2", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Multiple", "2", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Multiple", "2", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Multiple", "2", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "InProcess", "Single", "1", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Single", "1", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Single", "1", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Single", "1", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Multiple", "Single", "1", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Multiple", "Single", "1", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "InProcess", "Multiple", "1", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Multiple", "1", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Multiple", "1", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Multiple", "1", "AnyCpu", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Single", "1", "X86", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Single", "1", "X86", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Multiple", "Single", "1", "X86", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Multiple", "Single", "1", "X86", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Multiple", "1", "X86", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Multiple", "1", "X86", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Single", "2", "X86", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Single", "2", "X86", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Single", "2", "X86", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Multiple", "Single", "2", "X86", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Multiple", "Single", "2", "X86", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "InProcess", "Multiple", "2", "X86", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Multiple", "2", "X86", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Multiple", "2", "X86", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Multiple", "2", "X86", "ProjectFile", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Single", "2", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Single", "2", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Single", "2", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Multiple", "Single", "2", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Multiple", "Single", "2", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "InProcess", "Multiple", "2", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Multiple", "2", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Multiple", "2", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Multiple", "2", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "InProcess", "Single", "1", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Single", "1", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Single", "1", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Single", "1", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Multiple", "Single", "1", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Multiple", "Single", "1", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "InProcess", "Multiple", "1", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Multiple", "1", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Multiple", "1", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Multiple", "1", "AnyCpu", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Single", "1", "X86", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Single", "1", "X86", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Multiple", "Single", "1", "X86", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Multiple", "Single", "1", "X86", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Multiple", "1", "X86", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Multiple", "1", "X86", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Single", "2", "X86", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Single", "2", "X86", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Single", "2", "X86", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Multiple", "Single", "2", "X86", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Multiple", "Single", "2", "X86", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "InProcess", "Multiple", "2", "X86", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Multiple", "2", "X86", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Multiple", "2", "X86", "CmdArguments", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Multiple", "2", "X86", "CmdArguments", null)]
        public virtual void UserRunsTestsForSeveralAssemblies(string frameworkVersion, string process, string domain, string agents, string platform, string configurationType, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "3.4.1"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User runs tests for several assemblies", null, @__tags);
#line 7
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 3
this.FeatureBackground();
#line 8
    testRunner.Given(string.Format("Framework version is {0}", frameworkVersion), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
    testRunner.And("I created the folder mocks", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
    testRunner.And("I copied NUnit framework references to folder mocks", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
    testRunner.And("I added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for" +
                    " foo.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
    testRunner.And("I added NUnit framework references to foo.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
    testRunner.And("I compiled the assembly foo.tests to file mocks\\foo1.tests.dll", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
    testRunner.And("I added the assembly mocks\\foo1.tests.dll to the list of testing assemblies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
    testRunner.And("I compiled the assembly foo.tests to file mocks\\foo2.tests.dll", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
    testRunner.And("I added the assembly mocks\\foo2.tests.dll to the list of testing assemblies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
    testRunner.And("I compiled the assembly foo.tests to file mocks\\foo3.tests.dll", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
    testRunner.And("I added the assembly mocks\\foo3.tests.dll to the list of testing assemblies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
    testRunner.And("I compiled the assembly foo.tests to file mocks\\foo4.tests.dll", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
    testRunner.And("I added the assembly mocks\\foo4.tests.dll to the list of testing assemblies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 26
    testRunner.And("I compiled the assembly foo.tests to file mocks\\foo5.tests.dll", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
    testRunner.And("I added the assembly mocks\\foo5.tests.dll to the list of testing assemblies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
    testRunner.And("I compiled the assembly foo.tests to file mocks\\foo6.tests.dll", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
    testRunner.And("I added the assembly mocks\\foo6.tests.dll to the list of testing assemblies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
    testRunner.And(string.Format("I want to use {0} configuration type", configurationType), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
    testRunner.And("I added the arg workers=10 to NUnit console command line", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
    testRunner.And(string.Format("I added the arg agents={0} to NUnit console command line", agents), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
    testRunner.And(string.Format("I added the arg process={0} to NUnit console command line", process), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
    testRunner.And(string.Format("I added the arg domain={0} to NUnit console command line", domain), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
    testRunner.When("I run NUnit console", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 39
    testRunner.And("the output should contain correct set of TeamCity service messages", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table19 = new TechTalk.SpecFlow.Table(new string[] {
                        "field",
                        "value"});
            table19.AddRow(new string[] {
                        "Test Count",
                        "6"});
            table19.AddRow(new string[] {
                        "Passed",
                        "6"});
            table19.AddRow(new string[] {
                        "Failed",
                        "0"});
            table19.AddRow(new string[] {
                        "Inconclusive",
                        "0"});
            table19.AddRow(new string[] {
                        "Skipped",
                        "0"});
#line 40
    testRunner.And("the Test Run Summary should has following:", ((string)(null)), table19, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User runs parallelizable tests")]
        [NUnit.Framework.CategoryAttribute("3.4.1")]
        [NUnit.Framework.TestCaseAttribute("Version45", "InProcess", "Single", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Single", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Single", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Single", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Multiple", "Single", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Multiple", "Single", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "InProcess", "Multiple", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Multiple", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Multiple", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Multiple", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "InProcess", "Single", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Single", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Single", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Single", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Multiple", "Single", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Multiple", "Single", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "InProcess", "Multiple", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Multiple", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Multiple", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Multiple", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Single", "10", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Single", "10", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Multiple", "Single", "10", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Multiple", "Single", "10", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Multiple", "10", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Multiple", "10", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Single", "1", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Single", "1", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Multiple", "Single", "1", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Multiple", "Single", "1", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Multiple", "1", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Multiple", "1", "X86", null)]
        public virtual void UserRunsParallelizableTests(string frameworkVersion, string process, string domain, string agents, string platform, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "3.4.1"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User runs parallelizable tests", null, @__tags);
#line 119
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 3
this.FeatureBackground();
#line 120
    testRunner.Given(string.Format("Framework version is {0}", frameworkVersion), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 121
    testRunner.And("I added SuccessfulParallelizable method as SuccessfulParallelizable1 to the class" +
                    " Foo.Tests.UnitTests1 for foo1.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 122
    testRunner.And("I added SuccessfulParallelizable method as SuccessfulParallelizable2 to the class" +
                    " Foo.Tests.UnitTests1 for foo1.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 123
    testRunner.And("I added SuccessfulParallelizable method as SuccessfulParallelizable3 to the class" +
                    " Foo.Tests.UnitTests1 for foo1.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 124
    testRunner.And("I added attribute [assembly: NUnit.Framework.Parallelizable] to the assembly foo1" +
                    ".tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 125
    testRunner.And("I added attribute [NUnit.Framework.Parallelizable] to the class Foo.Tests.UnitTes" +
                    "ts1 for foo1.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 126
    testRunner.And("I added NUnit framework references to foo1.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 127
    testRunner.And("I added SuccessfulParallelizable method as SuccessfulParallelizable4 to the class" +
                    " Foo.Tests.UnitTests1 for foo2.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 128
    testRunner.And("I added SuccessfulParallelizable method as SuccessfulParallelizable5 to the class" +
                    " Foo.Tests.UnitTests1 for foo2.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 129
    testRunner.And("I added SuccessfulParallelizable method as SuccessfulParallelizable6 to the class" +
                    " Foo.Tests.UnitTests1 for foo2.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 130
    testRunner.And("I added attribute [assembly: NUnit.Framework.Parallelizable] to the assembly foo2" +
                    ".tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 131
    testRunner.And("I added attribute [NUnit.Framework.Parallelizable] to the class Foo.Tests.UnitTes" +
                    "ts1 for foo2.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 132
    testRunner.And("I added NUnit framework references to foo2.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
    testRunner.And("I created the folder mocks", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
    testRunner.And("I copied NUnit framework references to folder mocks", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 135
    testRunner.And(string.Format("I specified {0} platform for assembly foo1.tests", platform), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 136
    testRunner.And("I compiled the assembly foo1.tests to file mocks\\foo1.tests.dll", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 137
    testRunner.And(string.Format("I specified {0} platform for assembly foo2.tests", platform), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 138
    testRunner.And("I compiled the assembly foo2.tests to file mocks\\foo2.tests.dll", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 139
    testRunner.And("I added the assembly mocks\\foo1.tests.dll to the list of testing assemblies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 140
    testRunner.And("I added the assembly mocks\\foo2.tests.dll to the list of testing assemblies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 141
    testRunner.And("I want to use CmdArguments type of TeamCity integration", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 142
    testRunner.And("I added the arg workers=10 to NUnit console command line", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 143
    testRunner.And(string.Format("I added the arg agents={0} to NUnit console command line", agents), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 144
    testRunner.And(string.Format("I added the arg process={0} to NUnit console command line", process), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 145
    testRunner.And(string.Format("I added the arg domain={0} to NUnit console command line", domain), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 146
    testRunner.When("I run NUnit console", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 147
    testRunner.Then("the exit code should be 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 148
    testRunner.And("the output should contain correct set of TeamCity service messages", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table20 = new TechTalk.SpecFlow.Table(new string[] {
                        "field",
                        "value"});
            table20.AddRow(new string[] {
                        "Test Count",
                        "6"});
            table20.AddRow(new string[] {
                        "Passed",
                        "6"});
            table20.AddRow(new string[] {
                        "Failed",
                        "0"});
            table20.AddRow(new string[] {
                        "Inconclusive",
                        "0"});
            table20.AddRow(new string[] {
                        "Skipped",
                        "0"});
#line 149
    testRunner.And("the Test Run Summary should has following:", ((string)(null)), table20, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User runs parallelizable tests for NUnit 2 framework")]
        [NUnit.Framework.CategoryAttribute("3.4.1")]
        [NUnit.Framework.TestCaseAttribute("Version45", "InProcess", "Single", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Single", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Single", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Single", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Multiple", "Single", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Multiple", "Single", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "InProcess", "Multiple", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Multiple", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Multiple", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Multiple", "10", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "InProcess", "Single", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Single", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Single", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Single", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Multiple", "Single", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Multiple", "Single", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "InProcess", "Multiple", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "InProcess", "Multiple", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Multiple", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Multiple", "1", "AnyCpu", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Single", "10", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Single", "10", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Multiple", "Single", "10", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Multiple", "Single", "10", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Multiple", "10", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Multiple", "10", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Single", "1", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Single", "1", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Multiple", "Single", "1", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Multiple", "Single", "1", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version45", "Separate", "Multiple", "1", "X86", null)]
        [NUnit.Framework.TestCaseAttribute("Version40", "Separate", "Multiple", "1", "X86", null)]
        public virtual void UserRunsParallelizableTestsForNUnit2Framework(string frameworkVersion, string process, string domain, string agents, string platform, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "3.4.1"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User runs parallelizable tests for NUnit 2 framework", null, @__tags);
#line 192
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 3
this.FeatureBackground();
#line 193
    testRunner.And("I added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for" +
                    " foo.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 194
    testRunner.And("I added successfulCatA method as SuccessfulTestCatA to the class Foo.Tests.UnitTe" +
                    "sts1 for foo.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 195
    testRunner.And("I created the folder mocks", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 196
    testRunner.And("I added the reference ..\\..\\packages\\NUnit.2.6.4\\lib\\nunit.framework.dll to foo.t" +
                    "ests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 197
    testRunner.And("I copied the reference ..\\..\\packages\\NUnit.2.6.4\\lib\\nunit.framework.dll to fold" +
                    "er mocks", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 198
    testRunner.And(string.Format("I specified {0} platform for assembly foo.tests", platform), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 199
    testRunner.And("I compiled the assembly foo.tests to file mocks\\foo.tests.dll", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 200
    testRunner.And("I added the assembly mocks\\foo.tests.dll to the list of testing assemblies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 201
    testRunner.And("I added the arg Where=cat!=CatA to NUnit console command line", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 202
    testRunner.And("I want to use CmdArguments type of TeamCity integration", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 203
    testRunner.And("I added the arg workers=10 to NUnit console command line", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 204
    testRunner.And(string.Format("I added the arg agents={0} to NUnit console command line", agents), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 205
    testRunner.And(string.Format("I added the arg process={0} to NUnit console command line", process), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 206
    testRunner.And(string.Format("I added the arg domain={0} to NUnit console command line", domain), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 207
    testRunner.When("I run NUnit console", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 208
    testRunner.Then("the exit code should be 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 209
    testRunner.And("the output should contain correct set of TeamCity service messages", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table21 = new TechTalk.SpecFlow.Table(new string[] {
                        "field",
                        "value"});
            table21.AddRow(new string[] {
                        "Test Count",
                        "1"});
            table21.AddRow(new string[] {
                        "Passed",
                        "1"});
            table21.AddRow(new string[] {
                        "Failed",
                        "0"});
            table21.AddRow(new string[] {
                        "Inconclusive",
                        "0"});
            table21.AddRow(new string[] {
                        "Skipped",
                        "0"});
#line 210
    testRunner.And("the Test Run Summary should has following:", ((string)(null)), table21, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
