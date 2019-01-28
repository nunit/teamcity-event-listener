Feature: NUnit should running from different locations and appBase

Background:
    Given NUnit path is ..\nunit\

@3.4.1
Scenario Outline: I can run tests from nested directory
    Given Framework version is <frameworkVersion>	
    And I added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests
    And I created the folder mocks
    And I added NUnit framework references to foo.tests
    And I copied NUnit framework references to folder mocks
    And I compiled the assembly foo.tests to file mocks\foo.tests.dll	
    And I added the assembly mocks\foo.tests.dll to the list of testing assemblies
    And I want to use <configurationType> configuration type
    And I changed current directory to WorkingDirectory
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
    | configurationType | frameworkVersion |
    | CmdArguments      | Version45        |
    | CmdArguments      | Version40        |

@3.4.1
Scenario Outline: I can run tests from parent directory
    Given Framework version is <frameworkVersion>	
    And I added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests
    And I created the folder mocks
    And I added NUnit framework references to foo.tests
    And I copied NUnit framework references to folder mocks
    And I compiled the assembly foo.tests to file mocks\foo.tests.dll	
    And I added the assembly mocks\foo.tests.dll to the list of testing assemblies
    And I want to use <configurationType> configuration type
    And I changed current directory to ..\..\..\
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
    | configurationType | frameworkVersion |
    | ProjectFile       | Version45        |
    | ProjectFile       | Version40        |
    | CmdArguments      | Version45        |
    | CmdArguments      | Version40        |
