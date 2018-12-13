Feature: NUnit release OS resources

Background:
    Given NUnit path is ..\nunit\

@3.4.1
@ignore
Scenario Outline: Agent is finished when AppDomain are not unloaded correctly
    Given Framework version is <frameworkVersion>
    And I have added UnloadingDomain method as UnloadingDomain to the class Foo.Tests.UnitTests1 for foo.tests
    And I have created the folder mocks
    And I have added NUnit framework references to foo.tests
    And I have copied NUnit framework references to folder mocks
    And I have specified <platform> platform for assembly foo.tests
    And I have compiled the assembly foo.tests to file mocks\foo.tests.dll
    And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
    When I run NUnit console
    Then processes nunit-agent are finished
    Then processes nunit-agent-x86 are finished
Examples:
    | frameworkVersion | platform |
    | Version45         | AnyCpu   |
    | Version40         | AnyCpu   |
    | Version45         | X86      |
    | Version40         | X86      |