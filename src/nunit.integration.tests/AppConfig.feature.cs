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
    [NUnit.Framework.DescriptionAttribute("NUnit allows to load config files for tests")]
    public partial class NUnitAllowsToLoadConfigFilesForTestsFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "AppConfig.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "NUnit allows to load config files for tests", null, ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("I can the test with config file")]
        [NUnit.Framework.CategoryAttribute("3.4.1")]
        [NUnit.Framework.TestCaseAttribute("CmdArguments", "Version45", null)]
        [NUnit.Framework.TestCaseAttribute("CmdArguments", "Version40", null)]
        [NUnit.Framework.TestCaseAttribute("ProjectFile", "Version45", null)]
        [NUnit.Framework.TestCaseAttribute("ProjectFile", "Version40", null)]
        public virtual void ICanTheTestWithConfigFile(string configurationType, string frameworkVersion, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "3.4.1"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can the test with config file", null, @__tags);
#line 7
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 3
this.FeatureBackground();
#line 8
    testRunner.Given(string.Format("Framework version is {0}", frameworkVersion), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
    testRunner.And("I added successfulWithConfig method as SuccessfulTest to the class Foo.Tests.Unit" +
                    "Tests1 for foo.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
    testRunner.And("I created the folder mocks", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
    testRunner.And("I added NUnit framework references to foo.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
    testRunner.And("I copied NUnit framework references to folder mocks", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
    testRunner.And("I compiled the assembly foo.tests to file mocks\\foo.tests.dll", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
    testRunner.And("I added config file mocks\\foo.tests.dll.config", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
    testRunner.And("I added the assembly mocks\\foo.tests.dll to the list of testing assemblies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
    testRunner.And(string.Format("I want to use {0} configuration type", configurationType), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
    testRunner.When("I run NUnit console", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 18
    testRunner.Then("the exit code should be 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "field",
                        "value"});
            table1.AddRow(new string[] {
                        "Test Count",
                        "1"});
            table1.AddRow(new string[] {
                        "Passed",
                        "1"});
            table1.AddRow(new string[] {
                        "Failed",
                        "0"});
            table1.AddRow(new string[] {
                        "Inconclusive",
                        "0"});
            table1.AddRow(new string[] {
                        "Skipped",
                        "0"});
#line 19
    testRunner.And("the Test Run Summary should has following:", ((string)(null)), table1, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I can the test with config file for several assemblies using the command line for" +
            " the list of assemblies")]
        [NUnit.Framework.CategoryAttribute("3.4.1")]
        [NUnit.Framework.TestCaseAttribute("CmdArguments", "Version45", null)]
        [NUnit.Framework.TestCaseAttribute("CmdArguments", "Version40", null)]
        public virtual void ICanTheTestWithConfigFileForSeveralAssembliesUsingTheCommandLineForTheListOfAssemblies(string configurationType, string frameworkVersion, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "3.4.1"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can the test with config file for several assemblies using the command line for" +
                    " the list of assemblies", null, @__tags);
#line 34
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 3
this.FeatureBackground();
#line 35
    testRunner.Given(string.Format("Framework version is {0}", frameworkVersion), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 36
    testRunner.And("I added successfulWithConfig method as SuccessfulTest to the class Foo1.Tests.Uni" +
                    "tTests1 for foo1.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
    testRunner.And("I added successfulWithConfig method as SuccessfulTest to the class Foo2.Tests.Uni" +
                    "tTests1 for foo2.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
    testRunner.And("I created the folder mocks", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 39
    testRunner.And("I added NUnit framework references to foo1.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 40
    testRunner.And("I added NUnit framework references to foo2.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 41
    testRunner.And("I copied NUnit framework references to folder mocks", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 42
    testRunner.And("I compiled the assembly foo1.tests to file mocks\\foo1.tests.dll", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
    testRunner.And("I compiled the assembly foo2.tests to file mocks\\foo2.tests.dll", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
    testRunner.And("I added config file mocks\\foo1.tests.dll.config", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
    testRunner.And("I added config file mocks\\foo2.tests.dll.config", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
    testRunner.And("I added the assembly mocks\\foo1.tests.dll to the list of testing assemblies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
    testRunner.And("I added the assembly mocks\\foo2.tests.dll to the list of testing assemblies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
    testRunner.And(string.Format("I want to use {0} configuration type", configurationType), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
    testRunner.And("I added the arg Agents=0 to NUnit console command line", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
    testRunner.And("I added the arg TeamCity to NUnit console command line", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 51
    testRunner.When("I run NUnit console", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 52
    testRunner.Then("the exit code should be 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "field",
                        "value"});
            table2.AddRow(new string[] {
                        "Test Count",
                        "2"});
            table2.AddRow(new string[] {
                        "Passed",
                        "2"});
            table2.AddRow(new string[] {
                        "Failed",
                        "0"});
            table2.AddRow(new string[] {
                        "Inconclusive",
                        "0"});
            table2.AddRow(new string[] {
                        "Skipped",
                        "0"});
#line 53
    testRunner.And("the Test Run Summary should has following:", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "",
                        "name",
                        "out"});
            table3.AddRow(new string[] {
                        "flowStarted",
                        "",
                        ""});
            table3.AddRow(new string[] {
                        "testSuiteStarted",
                        "foo1.tests.dll",
                        ""});
            table3.AddRow(new string[] {
                        "flowStarted",
                        "",
                        ""});
            table3.AddRow(new string[] {
                        "testStarted",
                        "Foo1.Tests.UnitTests1.SuccessfulTest",
                        ""});
            table3.AddRow(new string[] {
                        "testStdOut",
                        "Foo1.Tests.UnitTests1.SuccessfulTest",
                        "foo1.tests.dll.config"});
            table3.AddRow(new string[] {
                        "testFinished",
                        "Foo1.Tests.UnitTests1.SuccessfulTest",
                        ""});
            table3.AddRow(new string[] {
                        "flowFinished",
                        "",
                        ""});
            table3.AddRow(new string[] {
                        "testSuiteFinished",
                        "foo1.tests.dll",
                        ""});
            table3.AddRow(new string[] {
                        "flowFinished",
                        "",
                        ""});
            table3.AddRow(new string[] {
                        "flowStarted",
                        "",
                        ""});
            table3.AddRow(new string[] {
                        "testSuiteStarted",
                        "foo2.tests.dll",
                        ""});
            table3.AddRow(new string[] {
                        "flowStarted",
                        "",
                        ""});
            table3.AddRow(new string[] {
                        "testStarted",
                        "Foo2.Tests.UnitTests1.SuccessfulTest",
                        ""});
            table3.AddRow(new string[] {
                        "testStdOut",
                        "Foo2.Tests.UnitTests1.SuccessfulTest",
                        "foo2.tests.dll.config"});
            table3.AddRow(new string[] {
                        "testFinished",
                        "Foo2.Tests.UnitTests1.SuccessfulTest",
                        ""});
            table3.AddRow(new string[] {
                        "flowFinished",
                        "",
                        ""});
            table3.AddRow(new string[] {
                        "testSuiteFinished",
                        "foo2.tests.dll",
                        ""});
            table3.AddRow(new string[] {
                        "flowFinished",
                        "",
                        ""});
#line 60
    testRunner.And("the output should contain TeamCity service messages:", ((string)(null)), table3, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I can the test with config file for several assemblies using the project file for" +
            " the list of assemblies")]
        [NUnit.Framework.CategoryAttribute("3.4.1")]
        [NUnit.Framework.TestCaseAttribute("ProjectFile", "Version45", null)]
        [NUnit.Framework.TestCaseAttribute("ProjectFile", "Version40", null)]
        public virtual void ICanTheTestWithConfigFileForSeveralAssembliesUsingTheProjectFileForTheListOfAssemblies(string configurationType, string frameworkVersion, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "3.4.1"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I can the test with config file for several assemblies using the project file for" +
                    " the list of assemblies", null, @__tags);
#line 86
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 3
this.FeatureBackground();
#line 87
    testRunner.Given(string.Format("Framework version is {0}", frameworkVersion), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 88
    testRunner.And("I added successfulWithConfig method as SuccessfulTest to the class Foo1.Tests.Uni" +
                    "tTests1 for foo1.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
    testRunner.And("I added successfulWithConfig method as SuccessfulTest to the class Foo2.Tests.Uni" +
                    "tTests1 for foo2.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
    testRunner.And("I created the folder mocks", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
    testRunner.And("I added NUnit framework references to foo1.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 92
    testRunner.And("I added NUnit framework references to foo2.tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 93
    testRunner.And("I copied NUnit framework references to folder mocks", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 94
    testRunner.And("I compiled the assembly foo1.tests to file mocks\\foo1.tests.dll", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 95
    testRunner.And("I compiled the assembly foo2.tests to file mocks\\foo2.tests.dll", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 96
    testRunner.And("I added config file mocks\\foo1.tests.dll.config", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 97
    testRunner.And("I added config file mocks\\foo2.tests.dll.config", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
    testRunner.And("I added the assembly mocks\\foo1.tests.dll to the list of testing assemblies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 99
    testRunner.And("I added the assembly mocks\\foo2.tests.dll to the list of testing assemblies", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 100
    testRunner.And(string.Format("I want to use {0} configuration type", configurationType), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
    testRunner.And("I added the arg Agents=0 to NUnit console command line", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 102
    testRunner.And("I added the arg TeamCity to NUnit console command line", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 103
    testRunner.When("I run NUnit console", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 104
    testRunner.Then("the exit code should be 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "field",
                        "value"});
            table4.AddRow(new string[] {
                        "Test Count",
                        "2"});
            table4.AddRow(new string[] {
                        "Passed",
                        "2"});
            table4.AddRow(new string[] {
                        "Failed",
                        "0"});
            table4.AddRow(new string[] {
                        "Inconclusive",
                        "0"});
            table4.AddRow(new string[] {
                        "Skipped",
                        "0"});
#line 105
    testRunner.And("the Test Run Summary should has following:", ((string)(null)), table4, "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "",
                        "name",
                        "out"});
            table5.AddRow(new string[] {
                        "flowStarted",
                        "",
                        ""});
            table5.AddRow(new string[] {
                        "testSuiteStarted",
                        "foo1.tests.dll",
                        ""});
            table5.AddRow(new string[] {
                        "flowStarted",
                        "",
                        ""});
            table5.AddRow(new string[] {
                        "testStarted",
                        "Foo1.Tests.UnitTests1.SuccessfulTest",
                        ""});
            table5.AddRow(new string[] {
                        "testStdOut",
                        "Foo1.Tests.UnitTests1.SuccessfulTest",
                        "foo1.tests.dll.config"});
            table5.AddRow(new string[] {
                        "testFinished",
                        "Foo1.Tests.UnitTests1.SuccessfulTest",
                        ""});
            table5.AddRow(new string[] {
                        "flowFinished",
                        "",
                        ""});
            table5.AddRow(new string[] {
                        "testSuiteFinished",
                        "foo1.tests.dll",
                        ""});
            table5.AddRow(new string[] {
                        "flowFinished",
                        "",
                        ""});
            table5.AddRow(new string[] {
                        "flowStarted",
                        "",
                        ""});
            table5.AddRow(new string[] {
                        "testSuiteStarted",
                        "foo2.tests.dll",
                        ""});
            table5.AddRow(new string[] {
                        "flowStarted",
                        "",
                        ""});
            table5.AddRow(new string[] {
                        "testStarted",
                        "Foo2.Tests.UnitTests1.SuccessfulTest",
                        ""});
            table5.AddRow(new string[] {
                        "testStdOut",
                        "Foo2.Tests.UnitTests1.SuccessfulTest",
                        "foo1.tests.dll.config"});
            table5.AddRow(new string[] {
                        "testFinished",
                        "Foo2.Tests.UnitTests1.SuccessfulTest",
                        ""});
            table5.AddRow(new string[] {
                        "flowFinished",
                        "",
                        ""});
            table5.AddRow(new string[] {
                        "testSuiteFinished",
                        "foo2.tests.dll",
                        ""});
            table5.AddRow(new string[] {
                        "flowFinished",
                        "",
                        ""});
#line 112
    testRunner.And("the output should contain TeamCity service messages:", ((string)(null)), table5, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
