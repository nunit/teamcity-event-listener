﻿Feature: NUnit allows to load config files for tests

Background:
    Given NUnit path is ..\nunit\
    
@3.4.1
Scenario Outline: I can the test with config file
    Given Framework version is <frameworkVersion>	
    And I added successfulWithConfig method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests
    And I created the folder mocks
    And I added NUnit framework references to foo.tests
    And I copied NUnit framework references to folder mocks
    And I compiled the assembly foo.tests to file mocks\foo.tests.dll
    And I added config file mocks\foo.tests.dll.config
    And I added the assembly mocks\foo.tests.dll to the list of testing assemblies		
    And I want to use <configurationType> configuration type
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
    | ProjectFile       | Version45        |
    | ProjectFile       | Version40        |

@3.4.1
Scenario Outline: I can the test with config file for several assemblies using the command line for the list of assemblies
    Given Framework version is <frameworkVersion>	
    And I added successfulWithConfig method as SuccessfulTest to the class Foo1.Tests.UnitTests1 for foo1.tests
    And I added successfulWithConfig method as SuccessfulTest to the class Foo2.Tests.UnitTests1 for foo2.tests
    And I created the folder mocks
    And I added NUnit framework references to foo1.tests
    And I added NUnit framework references to foo2.tests
    And I copied NUnit framework references to folder mocks
    And I compiled the assembly foo1.tests to file mocks\foo1.tests.dll
    And I compiled the assembly foo2.tests to file mocks\foo2.tests.dll
    And I added config file mocks\foo1.tests.dll.config
    And I added config file mocks\foo2.tests.dll.config
    And I added the assembly mocks\foo1.tests.dll to the list of testing assemblies
    And I added the assembly mocks\foo2.tests.dll to the list of testing assemblies
    And I want to use <configurationType> configuration type
    And I added the arg Agents=0 to NUnit console command line
    And I added the arg TeamCity to NUnit console command line
    When I run NUnit console
    Then the exit code should be 0
    And the Test Run Summary should has following:
    | field        | value |
    | Test Count   | 2     |
    | Passed       | 2     |
    | Failed       | 0     |
    | Inconclusive | 0     |
    | Skipped      | 0     |
    And the output should contain TeamCity service messages:
    |                   | name                                 | out                   |
    | flowStarted       |                                      |                       |
    | testSuiteStarted  | foo1.tests.dll                       |                       |
    | flowStarted       |                                      |                       |
    | testStarted       | Foo1.Tests.UnitTests1.SuccessfulTest |                       |
    | testStdOut        | Foo1.Tests.UnitTests1.SuccessfulTest | foo1.tests.dll.config |
    | testFinished      | Foo1.Tests.UnitTests1.SuccessfulTest |                       |
    | flowFinished      |                                      |                       |
    | testSuiteFinished | foo1.tests.dll                       |                       |
    | flowFinished      |                                      |                       |
    | flowStarted       |                                      |                       |
    | testSuiteStarted  | foo2.tests.dll                       |                       |
    | flowStarted       |                                      |                       |
    | testStarted       | Foo2.Tests.UnitTests1.SuccessfulTest |                       |
    | testStdOut        | Foo2.Tests.UnitTests1.SuccessfulTest | foo2.tests.dll.config |
    | testFinished      | Foo2.Tests.UnitTests1.SuccessfulTest |                       |
    | flowFinished      |                                      |                       |
    | testSuiteFinished | foo2.tests.dll                       |                       |
    | flowFinished      |                                      |                       |
Examples:
    | configurationType | frameworkVersion |
    | CmdArguments      | Version45        |
    | CmdArguments      | Version40        |
    
@3.4.1
Scenario Outline: I can the test with config file for several assemblies using the project file for the list of assemblies
    Given Framework version is <frameworkVersion>	
    And I added successfulWithConfig method as SuccessfulTest to the class Foo1.Tests.UnitTests1 for foo1.tests
    And I added successfulWithConfig method as SuccessfulTest to the class Foo2.Tests.UnitTests1 for foo2.tests
    And I created the folder mocks
    And I added NUnit framework references to foo1.tests
    And I added NUnit framework references to foo2.tests
    And I copied NUnit framework references to folder mocks
    And I compiled the assembly foo1.tests to file mocks\foo1.tests.dll
    And I compiled the assembly foo2.tests to file mocks\foo2.tests.dll
    And I added config file mocks\foo1.tests.dll.config
    And I added config file mocks\foo2.tests.dll.config
    And I added the assembly mocks\foo1.tests.dll to the list of testing assemblies
    And I added the assembly mocks\foo2.tests.dll to the list of testing assemblies
    And I want to use <configurationType> configuration type
    And I added the arg Agents=0 to NUnit console command line
    And I added the arg TeamCity to NUnit console command line
    When I run NUnit console
    Then the exit code should be 0
    And the Test Run Summary should has following:
    | field        | value |
    | Test Count   | 2     |
    | Passed       | 2     |
    | Failed       | 0     |
    | Inconclusive | 0     |
    | Skipped      | 0     |
    And the output should contain TeamCity service messages:
    |                   | name                                 | out                   |
    | flowStarted       |                                      |                       |
    | testSuiteStarted  | foo1.tests.dll                       |                       |
    | flowStarted       |                                      |                       |
    | testStarted       | Foo1.Tests.UnitTests1.SuccessfulTest |                       |
    | testStdOut        | Foo1.Tests.UnitTests1.SuccessfulTest | foo1.tests.dll.config |
    | testFinished      | Foo1.Tests.UnitTests1.SuccessfulTest |                       |
    | flowFinished      |                                      |                       |
    | testSuiteFinished | foo1.tests.dll                       |                       |
    | flowFinished      |                                      |                       |
    | flowStarted       |                                      |                       |
    | testSuiteStarted  | foo2.tests.dll                       |                       |
    | flowStarted       |                                      |                       |
    | testStarted       | Foo2.Tests.UnitTests1.SuccessfulTest |                       |
    | testStdOut        | Foo2.Tests.UnitTests1.SuccessfulTest | foo1.tests.dll.config |
    | testFinished      | Foo2.Tests.UnitTests1.SuccessfulTest |                       |
    | flowFinished      |                                      |                       |
    | testSuiteFinished | foo2.tests.dll                       |                       |
    | flowFinished      |                                      |                       |
Examples:
    | configurationType | frameworkVersion |
    | ProjectFile       | Version45        |
    | ProjectFile       | Version40        |