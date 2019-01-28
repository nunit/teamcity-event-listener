Feature: NUnit should support excluding and including tests

Background:
    Given NUnit path is ..\nunit\

@3.4.1
@teamcity
Scenario Outline: I can run all tests except those in the CatA category
    Given Framework version is <frameworkVersion>	
    And I added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests
    And I added successfulCatA method as SuccessfulTestCatA to the class Foo.Tests.UnitTests1 for foo.tests	
    And I created the folder mocks
    And I added NUnit framework references to foo.tests
    And I copied NUnit framework references to folder mocks
    And I compiled the assembly foo.tests to file mocks\foo.tests.dll	
    And I added the assembly mocks\foo.tests.dll to the list of testing assemblies
    And I added the arg Where=cat!=CatA to NUnit console command line
    When I run NUnit console
    Then the exit code should be 0
    And the Test Run Summary should has following:
    | field        | value |
    | Test Count   | 1     |
    | Passed       | 1     |
    | Failed       | 0     |
    | Inconclusive | 0     |
    | Skipped      | 0     |
Examples:
    | frameworkVersion |
    | Version45        |
    | Version40        |

@3.4.1
@teamcity
Scenario Outline: I can run only the tests in the CatA category
    Given Framework version is <frameworkVersion>	
    And I added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests	
    And I added successfulCatA method as SuccessfulTestCatA to the class Foo.Tests.UnitTests1 for foo.tests	
    And I added failed method as FailedTest to the class Foo.Tests.UnitTests1 for foo.tests
    And I added ignored method as IgnoredTest to the class Foo.Tests.UnitTests1 for foo.tests
    And I created the folder mocks
    And I added NUnit framework references to foo.tests
    And I copied NUnit framework references to folder mocks
    And I compiled the assembly foo.tests to file mocks\foo.tests.dll	
    And I added the assembly mocks\foo.tests.dll to the list of testing assemblies
    And I added the arg Where=cat==CatA to NUnit console command line
    When I run NUnit console
    Then the exit code should be 0
    And the Test Run Summary should has following:
    | field        | value |
    | Test Count   | 1     |
    | Passed       | 1     |
    | Failed       | 0     |
    | Inconclusive | 0     |
    | Skipped      | 0     |
Examples:
    | frameworkVersion |
    | Version45        |
    | Version40        |
    
@3.4.1
@teamcity
Scenario Outline: I can run all tests except those in the CatA category from NUnit 2 framework
    Given Framework version is <frameworkVersion>	
    And I added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests
    And I added successfulCatA method as SuccessfulTestCatA to the class Foo.Tests.UnitTests1 for foo.tests	
    And I created the folder mocks
    And I added the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to foo.tests
    And I copied the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to folder mocks
    And I compiled the assembly foo.tests to file mocks\foo.tests.dll	
    And I added the assembly mocks\foo.tests.dll to the list of testing assemblies
    And I added the arg Where=cat!=CatA to NUnit console command line
    When I run NUnit console
    Then the exit code should be 0
    And the Test Run Summary should has following:
    | field        | value |
    | Test Count   | 1     |
    | Passed       | 1     |
    | Failed       | 0     |
    | Inconclusive | 0     |
    | Skipped      | 0     |
Examples:
    | frameworkVersion |
    | Version45        |
    | Version40        |