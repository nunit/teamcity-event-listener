Feature: NUnit should support platforms

Background:
    Given NUnit path is ..\nunit\

@3.4.1
Scenario Outline: I can run test for different platforms
    Given Framework version is <frameworkVersion>	
    And I added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests	
    And I created the folder mocks
    And I added NUnit framework references to foo.tests
    And I copied NUnit framework references to folder mocks
    And I specified <platform> platform for assembly foo.tests
    And I compiled the assembly foo.tests to file mocks\foo.tests.dll	
    And I added the assembly mocks\foo.tests.dll to the list of testing assemblies
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
    | frameworkVersion | platform |
    | Version45         | AnyCpu   |
    | Version40         | AnyCpu   |
    | Version20         | AnyCpu   |
    | Version45         | X86      |
    | Version40         | X86      |
    | Version20         | X86      |
    #| Version45         | X64      |
    #| Version40         | X64      |
    #| Version20         | X64      |
