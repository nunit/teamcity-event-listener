Feature: NUnit should support TeamCity	

Background:
	Given NUnit path is ..\nunit\

@3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run successful test for NUnit3
	Given Framework version is <frameworkVersion>
	And I have added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll	
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                                | captureStandardOutput | duration | flowId | parent | message | details | out    | tc:tags                       |
	| flowStarted       |                                     |                       |          | .+     | .+     |         |         |        |                               |
	| testSuiteStarted  | foo.tests.dll                       |                       |          | .+     |        |         |         |        |                               |
	| flowStarted       |                                     |                       |          | .+     | .+     |         |         |        |                               |
	| testStarted       | Foo.Tests.UnitTests1.SuccessfulTest | false                 |          | .+     |        |         |         |        |                               |
	| testStdOut        | Foo.Tests.UnitTests1.SuccessfulTest |                       |          | .+     |        |         |         | output | tc:parseServiceMessagesInside |
	| testFinished      | Foo.Tests.UnitTests1.SuccessfulTest |                       | \d+      | .+     |        |         |         |        |                               |
	| flowFinished      |                                     |                       |          | .+     |        |         |         |        |                               |
	| testSuiteFinished | foo.tests.dll                       |                       |          | .+     |        |         |         |        |                               |
	| flowFinished      |                                     |                       |          | .+     |        |         |         |        |                               |
Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |

@3.4.1
@teamcity
Scenario: Use root flow id from environment variable TEAMCITY_PROCESS_FLOW_ID
	Given Framework version is Version45
	And I have added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	And I have added the environment variable TEAMCITY_PROCESS_FLOW_ID as abc
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                                | captureStandardOutput | duration | flowId | parent | message | details | out    | tc:tags                       |
	| flowStarted       |                                     |                       |          | .+     | abc    |         |         |        |                               |
	| testSuiteStarted  | foo.tests.dll                       |                       |          | .+     |        |         |         |        |                               |
	| flowStarted       |                                     |                       |          | .+     | .+     |         |         |        |                               |
	| testStarted       | Foo.Tests.UnitTests1.SuccessfulTest | false                 |          | .+     |        |         |         |        |                               |
	| testStdOut        | Foo.Tests.UnitTests1.SuccessfulTest |                       |          | .+     |        |         |         | output | tc:parseServiceMessagesInside |
	| testFinished      | Foo.Tests.UnitTests1.SuccessfulTest |                       | \d+      | .+     |        |         |         |        |                               |
	| flowFinished      |                                     |                       |          | .+     |        |         |         |        |                               |
	| testSuiteFinished | foo.tests.dll                       |                       |          | .+     |        |         |         |        |                               |
	| flowFinished      |                                     |                       |          | .+     |        |         |         |        |                               |

@3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run test with Assert.Pass
	Given Framework version is <frameworkVersion>	
	And I have added Pass method as PassTest to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                          | captureStandardOutput | duration | flowId | parent | message | details | out | tc:tags |
	| flowStarted       |                               |                       |          | .+     | .+     |         |         |     |         |
	| testSuiteStarted  | foo.tests.dll                 |                       |          | .+     |        |         |         |     |         |
	| flowStarted       |                               |                       |          | .+     | .+     |         |         |     |         |
	| testStarted       | Foo.Tests.UnitTests1.PassTest | false                 |          | .+     |        |         |         |     |         |
	| testFinished      | Foo.Tests.UnitTests1.PassTest |                       | \d+      | .+     |        |         |         |     |         |
	| flowFinished      |                               |                       |          | .+     |        |         |         |     |         |
	| testSuiteFinished | foo.tests.dll                 |                       |          | .+     |        |         |         |     |         |
	| flowFinished      |                               |                       |          | .+     |        |         |         |     |         |
Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |


@3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run test with Assert.Pass with text
	Given Framework version is <frameworkVersion>	
	And I have added PassWithText method as PassTest to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                          | captureStandardOutput | duration | flowId | parent | message | details | out                            | tc:tags                       |
	| flowStarted       |                               |                       |          | .+     | .+     |         |         |                                |                               |
	| testSuiteStarted  | foo.tests.dll                 |                       |          | .+     |        |         |         |                                |                               |
	| flowStarted       |                               |                       |          | .+     | .+     |         |         |                                |                               |
	| testStarted       | Foo.Tests.UnitTests1.PassTest | false                 |          | .+     |        |         |         |                                |                               |
	| testStdOut        | Foo.Tests.UnitTests1.PassTest |                       |          | .+     |        |         |         | Assert.Pass message: some text | tc:parseServiceMessagesInside |
	| testFinished      | Foo.Tests.UnitTests1.PassTest |                       | \d+      | .+     |        |         |         |                                |                               |
	| flowFinished      |                               |                       |          | .+     |        |         |         |                                |                               |
	| testSuiteFinished | foo.tests.dll                 |                       |          | .+     |        |         |         |                                |                               |
	| flowFinished      |                               |                       |          | .+     |        |         |         |                                |                               |
Examples:
	| frameworkVersion |
	| Version45        |


@3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run test with Assert.Pass for NUnit2
	Given Framework version is <frameworkVersion>
	And I have added Pass method as PassTest to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have copied the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to folder mocks
	And I have added the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to foo.tests
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                          | captureStandardOutput | duration | flowId | parent | message | details | out    | tc:tags                       |
	| testSuiteStarted  | foo.tests.dll                 |                       |          | .+     |        |         |         |        |                               |
	| testStarted       | Foo.Tests.UnitTests1.PassTest | false                 |          | .+     |        |         |         |        |                               |
	| testFinished      | Foo.Tests.UnitTests1.PassTest |                       | \d+      | .+     |        |         |         |        |                               |
	| testSuiteFinished | foo.tests.dll                 |                       |          | .+     |        |         |         |        |                               |
Examples:
	| frameworkVersion |
	| Version45        |

@3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run test with Assert.Pass with text for NUnit2
	Given Framework version is <frameworkVersion>
	And I have added PassWithText method as PassTest to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have copied the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to folder mocks
	And I have added the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to foo.tests
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                          | captureStandardOutput | duration | flowId | parent | message | details | out                            | tc:tags                       |
	| testSuiteStarted  | foo.tests.dll                 |                       |          | .+     |        |         |         |                                |                               |
	| testStarted       | Foo.Tests.UnitTests1.PassTest | false                 |          | .+     |        |         |         |                                |                               |
	| testStdOut        | Foo.Tests.UnitTests1.PassTest |                       |          | .+     |        |         |         | Assert.Pass message: some text | tc:parseServiceMessagesInside |
	| testFinished      | Foo.Tests.UnitTests1.PassTest |                       | \d+      | .+     |        |         |         |                                |                               |
	| testSuiteFinished | foo.tests.dll                 |                       |          | .+     |        |         |         |                                |                               |
Examples:
	| frameworkVersion |
	| Version45        |

@3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run it for different types of tests
	Given Framework version is <frameworkVersion>	
	And I have added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests	
	And I have added failed method as FailedTest to the class Foo.Tests.UnitTests2 for foo.tests
	And I have added ignored method as IgnoredTest to the class Foo.Tests.UnitTests3 for foo.tests
	And I have added inconclusive method as InconclusiveTest to the class Foo.Tests.UnitTests4 for foo.tests
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll	
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use <teamCityIntegration> type of TeamCity integration
	And I want to use <configurationType> configuration type
	When I run NUnit console
	Then the exit code should be 1
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                                  | captureStandardOutput | duration | flowId | parent | message      | details                           | out    | tc:tags                       |
	| flowStarted       |                                       |                       |          | .+     | .+     |              |                                   |        |                               |
	| testSuiteStarted  | foo.tests.dll                         |                       |          | .+     |        |              |                                   |        |                               |
	| flowStarted       |                                       |                       |          | .+     | .+     |              |                                   |        |                               |
	| testStarted       | Foo.Tests.UnitTests1.SuccessfulTest   | false                 |          | .+     |        |              |                                   |        |                               |
	| testStdOut        | Foo.Tests.UnitTests1.SuccessfulTest   |                       |          | .+     |        |              |                                   | output | tc:parseServiceMessagesInside |
	| testFinished      | Foo.Tests.UnitTests1.SuccessfulTest   |                       | \d+      | .+     |        |              |                                   |        |                               |
	| flowFinished      |                                       |                       |          | .+     |        |              |                                   |        |                               |
	| flowStarted       |                                       |                       |          | .+     | .+     |              |                                   |        |                               |
	| testStarted       | Foo.Tests.UnitTests2.FailedTest       | false                 |          | .+     |        |              |                                   |        |                               |
	| testFailed        | Foo.Tests.UnitTests2.FailedTest       |                       |          | .+     |        | Reason       | Foo.Tests.UnitTests2.FailedTest() |        |                               |
	| testFinished      | Foo.Tests.UnitTests2.FailedTest       |                       | \d+      | .+     |        |              |                                   |        |                               |
	| flowFinished      |                                       |                       |          | .+     |        |              |                                   |        |                               |
	| flowStarted       |                                       |                       |          | .+     | .+     |              |                                   |        |                               |
	| testStarted       | Foo.Tests.UnitTests3.IgnoredTest      | false                 |          | .+     |        |              |                                   |        |                               |
	| testIgnored       | Foo.Tests.UnitTests3.IgnoredTest      |                       |          | .+     |        | Reason       |                                   |        |                               |
	| flowFinished      |                                       |                       |          | .+     |        |              |                                   |        |                               |
	| flowStarted       |                                       |                       |          | .+     | .+     |              |                                   |        |                               |
	| testStarted       | Foo.Tests.UnitTests4.InconclusiveTest | false                 |          | .+     |        |              |                                   |        |                               |
	| testIgnored       | Foo.Tests.UnitTests4.InconclusiveTest |                       |          | .+     |        | Inconclusive |                                   |        |                               |
	| flowFinished      |                                       |                       |          | .+     |        |              |                                   |        |                               |
	| testSuiteFinished | foo.tests.dll                         |                       |          | .+     |        |              |                                   |        |                               |
	| flowFinished      |                                       |                       |          | .+     |        |              |                                   |        |                               |
Examples:
	| configurationType | frameworkVersion | teamCityIntegration |
	| ProjectFile       | Version45        | CmdArguments        |
	| ProjectFile       | Version40        | CmdArguments        |
	| CmdArguments      | Version45        | CmdArguments        |
	| CmdArguments      | Version40        | CmdArguments        |
	| ProjectFile       | Version45        | EnvVariable         |
	| ProjectFile       | Version40        | EnvVariable         |
	| CmdArguments      | Version45        | EnvVariable         |
	| CmdArguments      | Version40        | EnvVariable         |

@3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run it for failed setup
	Given Framework version is <frameworkVersion>	
	And I have added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests	
	And I have added failedSetUp method as FailedSetUp to the class Foo.Tests.UnitTests1 for foo.tests	
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll	
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	When I run NUnit console
	Then the exit code should be 1
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                                | captureStandardOutput | duration | flowId | parent | message          | details                            | out |
	| flowStarted       |                                     |                       |          | .+     | .+     |                  |                                    |     |
	| testSuiteStarted  | foo.tests.dll                       |                       |          | .+     |        |                  |                                    |     |
	| flowStarted       |                                     |                       |          | .+     | .+     |                  |                                    |     |
	| testStarted       | Foo.Tests.UnitTests1.SuccessfulTest | false                 |          | .+     |        |                  |                                    |     |
	| testFailed        | Foo.Tests.UnitTests1.SuccessfulTest |                       |          | .+     |        | System.Exception | Foo.Tests.UnitTests1.FailedSetUp() |     |
	| testFinished      | Foo.Tests.UnitTests1.SuccessfulTest |                       | \d+      | .+     |        |                  |                                    |     |
	| flowFinished      |                                     |                       |          | .+     |        |                  |                                    |     |
	| testSuiteFinished | foo.tests.dll                       |                       |          | .+     |        |                  |                                    |     |
	| flowFinished      |                                     |                       |          | .+     |        |                  |                                    |     |
Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |

@3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run it for failed one time setup
	Given Framework version is <frameworkVersion>	
	And I have added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests	
	And I have added failedSetUp method as FailedOneTimeSetUp to the class Foo.Tests.UnitTests1 for foo.tests	
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll	
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	When I run NUnit console
	Then the exit code should be 1
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                                | captureStandardOutput | duration | flowId | parent | message          | details                            | out |
	| flowStarted       |                                     |                       |          | .+     | .+     |                  |                                    |     |
	| testSuiteStarted  | foo.tests.dll                       |                       |          | .+     |        |                  |                                    |     |
	| flowStarted       |                                     |                       |          | .+     | .+     |                  |                                    |     |
	| testStarted       | Foo.Tests.UnitTests1.SuccessfulTest | false                 |          | .+     |        |                  |                                    |     |
	| testFailed        | Foo.Tests.UnitTests1.SuccessfulTest |                       |          | .+     |        | System.Exception | Foo.Tests.UnitTests1.FailedOneTimeSetUp() |     |
	| testFinished      | Foo.Tests.UnitTests1.SuccessfulTest |                       | \d+      | .+     |        |                  |                                    |     |
	| flowFinished      |                                     |                       |          | .+     |        |                  |                                    |     |
	| testSuiteFinished | foo.tests.dll                       |                       |          | .+     |        |                  |                                    |     |
	| flowFinished      |                                     |                       |          | .+     |        |                  |                                    |     |
Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |

@3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run it for failed tear down
	Given Framework version is <frameworkVersion>
	And I have added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added failedTearDown method as FailedTearDown to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	When I run NUnit console
	Then the exit code should be 1
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                                | captureStandardOutput | duration | flowId | parent | message          | details                               | out    | tc:tags                       |
	| flowStarted       |                                     |                       |          | .+     | .+     |                  |                                       |        |                               |
	| testSuiteStarted  | foo.tests.dll                       |                       |          | .+     |        |                  |                                       |        |                               |
	| flowStarted       |                                     |                       |          | .+     | .+     |                  |                                       |        |                               |
	| testStarted       | Foo.Tests.UnitTests1.SuccessfulTest | false                 |          | .+     |        |                  |                                       |        |                               |
	| testFailed        | Foo.Tests.UnitTests1.SuccessfulTest |                       |          | .+     |        | System.Exception | Foo.Tests.UnitTests1.FailedTearDown() |        |                               |
	| testStdOut        | Foo.Tests.UnitTests1.SuccessfulTest |                       |          | .+     |        |                  |                                       | output | tc:parseServiceMessagesInside |
	| testFinished      | Foo.Tests.UnitTests1.SuccessfulTest |                       | \d+      | .+     |        |                  |                                       |        |                               |
	| flowFinished      |                                     |                       |          | .+     |        |                  |                                       |        |                               |
	| testSuiteFinished | foo.tests.dll                       |                       |          | .+     |        |                  |                                       |        |                               |
	| flowFinished      |                                     |                       |          | .+     |        |                  |                                       |        |                               |
Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |

@3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run it for failed ctor
	Given Framework version is <frameworkVersion>	
	And I have added throwException method as ThrowException to the Ctor of class Foo.Tests.UnitTests1 for foo.tests
	And I have added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll	
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	When I run NUnit console
	Then the exit code should be 1
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                                | captureStandardOutput | duration | flowId | parent | message                      | details                                                                              | out |
	| flowStarted       |                                     |                       |          |        |        |                              |                                                                                      |     |
	| testSuiteStarted  | foo.tests.dll                       |                       |          | .+     |        |                              |                                                                                      |     |
	| testStarted       | Foo.Tests.UnitTests1.SuccessfulTest | false                 |          | .+     |        |                              |                                                                                      |     |
	| testFailed        | Foo.Tests.UnitTests1.SuccessfulTest |                       |          | .+     |        | System.Exception : Exception | at Foo\.Tests\.UnitTests1\.ThrowException\(\).+at Foo\.Tests\.UnitTests1\.\.ctor\(\) |     |
	| testFinished      | Foo.Tests.UnitTests1.SuccessfulTest |                       | \d+      | .+     |        |                              |                                                                                      |     |
	| testSuiteFinished | foo.tests.dll                       |                       |          | .+     |        |                              |                                                                                      |     |
	| flowFinished      |                                     |                       |          | .+     |        |                              |                                                                                      |     |
Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |

@3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run it for failed ctor for NUnit2
	Given Framework version is <frameworkVersion>
	And I have added throwException method as ThrowException to the Ctor of class Foo.Tests.UnitTests1 for foo.tests
	And I have added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have copied the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to folder mocks
	And I have added the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to foo.tests
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	When I run NUnit console
	Then the exit code should be 1
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                                | captureStandardOutput | duration | flowId | parent | message                                | details                                                                       | out |
	| testSuiteStarted  | foo.tests.dll                       |                       |          | .+     |        |                                        |                                                                               |     |
	| testStarted       | Foo.Tests.UnitTests1.SuccessfulTest | false                 |          | .+     |        |                                        |                                                                               |     |
	| testFailed        | Foo.Tests.UnitTests1.SuccessfulTest |                       |          | .+     |        | SetUp \: System.Exception \: Exception | at Foo.Tests.UnitTests1.ThrowException().*at Foo\.Tests\.UnitTests1\.\.ctor() |     |
	| testFinished      | Foo.Tests.UnitTests1.SuccessfulTest |                       | \d+      | .+     |        |                                        |                                                                               |     |
	| testSuiteFinished | foo.tests.dll                       |                       |          | .+     |        |                                        |                                                                               |     |
Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |

@3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run it for parallelizable tests
	Given Framework version is <frameworkVersion>	        
	And I have added SuccessfulParallelizable method as SuccessfulParallelizable1 to the class Foo.Tests.UnitTests1 for foo1.tests	
	And I have added SuccessfulParallelizable method as SuccessfulParallelizable2 to the class Foo.Tests.UnitTests1 for foo1.tests	
	And I have added SuccessfulParallelizable method as SuccessfulParallelizable3 to the class Foo.Tests.UnitTests1 for foo1.tests	
	And I have added attribute [assembly: NUnit.Framework.Parallelizable] to the assembly foo1.tests
	And I have added attribute [NUnit.Framework.Parallelizable] to the class Foo.Tests.UnitTests1 for foo1.tests
	And I have added NUnit framework references to foo1.tests
	And I have added SuccessfulParallelizable method as SuccessfulParallelizable4 to the class Foo.Tests.UnitTests1 for foo2.tests	
	And I have added SuccessfulParallelizable method as SuccessfulParallelizable5 to the class Foo.Tests.UnitTests1 for foo2.tests	
	And I have added SuccessfulParallelizable method as SuccessfulParallelizable6 to the class Foo.Tests.UnitTests1 for foo2.tests	
	And I have added attribute [assembly: NUnit.Framework.Parallelizable] to the assembly foo2.tests
	And I have added attribute [NUnit.Framework.Parallelizable] to the class Foo.Tests.UnitTests1 for foo2.tests
	And I have added NUnit framework references to foo2.tests
	And I have created the folder mocks	
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo1.tests to file mocks\foo1.tests.dll	
	And I have compiled the assembly foo2.tests to file mocks\foo2.tests.dll	
	And I have added the assembly mocks\foo1.tests.dll to the list of testing assemblies
	And I have added the assembly mocks\foo2.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	And I have added the arg workers=10 to NUnit console command line
	And I have added the arg agents=10 to NUnit console command line
	When I run NUnit console
	Then the exit code should be 0
	And the Test Run Summary should has following:
	| field        | value |
	| Test Count   | 6     |
	| Passed       | 6     |
	| Failed       | 0     |
	| Inconclusive | 0     |
	| Skipped      | 0     |
Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |

@3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run successful tests with the same names in the several assemblies
	Given Framework version is <frameworkVersion>	
	And I have added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo1.tests
	And I have added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo2.tests
	And I have created the folder mocks
	And I have added NUnit framework references to foo1.tests
	And I have added NUnit framework references to foo2.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo1.tests to file mocks\foo1.tests.dll	
	And I have compiled the assembly foo2.tests to file mocks\foo2.tests.dll	
	And I have added the assembly mocks\foo1.tests.dll to the list of testing assemblies
	And I have added the assembly mocks\foo2.tests.dll to the list of testing assemblies
	And I have added the arg workers=1 to NUnit console command line
	And I have added the arg agents=1 to NUnit console command line
	And I want to use CmdArguments type of TeamCity integration
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                                | captureStandardOutput | duration | flowId | parent | message | details | out    | tc:tags                       |
	| flowStarted       |                                     |                       |          | .+     | .+     |         |         |        |                               |
	| testSuiteStarted  | foo1.tests.dll                      |                       |          | .+     |        |         |         |        |                               |
	| flowStarted       |                                     |                       |          | .+     | .+     |         |         |        |                               |
	| testStarted       | Foo.Tests.UnitTests1.SuccessfulTest | false                 |          | .+     |        |         |         |        |                               |
	| testStdOut        | Foo.Tests.UnitTests1.SuccessfulTest |                       |          | .+     |        |         |         | output | tc:parseServiceMessagesInside |
	| testFinished      | Foo.Tests.UnitTests1.SuccessfulTest |                       | \d+      | .+     |        |         |         |        |                               |
	| flowFinished      |                                     |                       |          | .+     |        |         |         |        |                               |
	| testSuiteFinished | foo1.tests.dll                      |                       |          | .+     |        |         |         |        |                               |
	| flowFinished      |                                     |                       |          | .+     |        |         |         |        |                               |
	| flowStarted       |                                     |                       |          | .+     | .+     |         |         |        |                               |
	| testSuiteStarted  | foo2.tests.dll                      |                       |          | .+     |        |         |         |        |                               |
	| flowStarted       |                                     |                       |          | .+     | .+     |         |         |        |                               |
	| testStarted       | Foo.Tests.UnitTests1.SuccessfulTest | false                 |          | .+     |        |         |         |        |                               |
	| testStdOut        | Foo.Tests.UnitTests1.SuccessfulTest |                       |          | .+     |        |         |         | output |                               |
	| testFinished      | Foo.Tests.UnitTests1.SuccessfulTest |                       | \d+      | .+     |        |         |         |        |                               |
	| flowFinished      |                                     |                       |          | .+     |        |         |         |        |                               |
	| testSuiteFinished | foo2.tests.dll                      |                       |          | .+     |        |         |         |        |                               |
	| flowFinished      |                                     |                       |          | .+     |        |         |         |        |                               |
Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |

@3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run many test
	Given Framework version is <frameworkVersion>	
	And I have added 1000 successful methods as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests1
	And I have added 1000 successful methods as SuccessfulTest to the class Foo.Tests.UnitTests2 for foo.tests2
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests1
	And I have added NUnit framework references to foo.tests2
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests1 to file mocks\foo.tests1.dll	
	And I have compiled the assembly foo.tests2 to file mocks\foo.tests2.dll
	And I have added the assembly mocks\foo.tests1.dll to the list of testing assemblies
	And I have added the assembly mocks\foo.tests2.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |

@dev
@teamcity
@ignore
Scenario Outline: NUnit sends TeamCity's service messages when I run many test for several assemblies for NUnit2
	Given Framework version is <frameworkVersion>	
	And I have added 100 successful methods as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests1
	And I have added 100 successful methods as SuccessfulTest to the class Foo.Tests.UnitTests2 for foo.tests2
	And I have created the folder mocks
	And I have copied the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to folder mocks
	And I have added the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to foo.tests1
	And I have added the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to foo.tests2
	And I have compiled the assembly foo.tests1 to file mocks\foo.tests1.dll	
	And I have compiled the assembly foo.tests2 to file mocks\foo.tests2.dll
	And I have added the assembly mocks\foo.tests1.dll to the list of testing assemblies
	And I have added the assembly mocks\foo.tests2.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	And I have added the arg workers=10 to NUnit console command line
	And I have added the arg agents=<agents> to NUnit console command line
	And I have added the arg process=<process> to NUnit console command line
	And I have added the arg domain=<domain> to NUnit console command line
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
Examples:
	| frameworkVersion | process   | domain   | agents |
	| Version45        | InProcess | None     | 10     |
	| Version40        | InProcess | None     | 10     |
	| Version45        | Separate  | None     | 10     |
	| Version45        | Multiple  | None     | 10     |
	| Version45        | InProcess | Single   | 10     |
	| Version45        | Separate  | Single   | 10     |
	| Version45        | Multiple  | Single   | 10     |
	| Version45        | InProcess | Multiple | 10     |
	| Version45        | Separate  | Multiple | 10     |
	| Version45        | InProcess | None     | 1      |
	| Version45        | Separate  | None     | 1      |
	| Version45        | Multiple  | None     | 1      |
	| Version45        | InProcess | Single   | 1      |
	| Version45        | Separate  | Single   | 1      |
	| Version45        | Multiple  | Single   | 1      |
	| Version45        | InProcess | Multiple | 1      |
	| Version45        | Separate  | Multiple | 1      |

@3.4.1
@ignore
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages for bunch of test for several assemblies for NUnit2
	Given Framework version is <frameworkVersion>
	And I have created the folder mocks
	And I have copied the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to folder mocks	
	And I have created assemblies according to NUnit2 test results ..\..\..\testsData\NUnit2HugeTestResult.xml
	And I have added the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to MAP.Common.Test
	And I have compiled the assembly MAP.Common.Test to file mocks\MAP.Common.Test.dll
	And I have added the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to MAP.Web.Test
	And I have compiled the assembly MAP.Web.Test to file mocks\MAP.Web.Test.dll
	And I have added the assembly mocks\MAP.Common.Test.dll to the list of testing assemblies
	And I have added the assembly mocks\MAP.Web.Test.dll to the list of testing assemblies	
	And I want to use CmdArguments type of TeamCity integration
	And I have added the arg workers=10 to NUnit console command line
	And I have added the arg agents=<agents> to NUnit console command line
	And I have added the arg process=<process> to NUnit console command line
	And I have added the arg domain=<domain> to NUnit console command line
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
Examples:
	| frameworkVersion | process   | domain   | agents |
	| Version45        | InProcess | None     | 10     |
	| Version40        | InProcess | None     | 10     |
	| Version45        | Separate  | None     | 10     |
	| Version45        | Multiple  | None     | 10     |
	| Version45        | InProcess | Single   | 10     |
	| Version45        | Separate  | Single   | 10     |
	| Version45        | Multiple  | Single   | 10     |
	| Version45        | InProcess | Multiple | 10     |
	| Version45        | Separate  | Multiple | 10     |
	| Version45        | InProcess | None     | 1      |
	| Version45        | Separate  | None     | 1      |
	| Version45        | Multiple  | None     | 1      |
	| Version45        | InProcess | Single   | 1      |
	| Version45        | Separate  | Single   | 1      |
	| Version45        | Multiple  | Single   | 1      |
	| Version45        | InProcess | Multiple | 1      |
	| Version45        | Separate  | Multiple | 1      |

@3.4.1
@teamcity
Scenario: NUnit show version and extensions when users pass --list-extensions --teamcity args
	Given I have added the arg ListExtensions to NUnit console command line
	And I have added the arg TeamCity to NUnit console command line
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain lines:
	|                                                                              |
	| \\s*NUnit\\sConsole\\sRunner\\s\\d+\\.\\d+\\.\\d+\\s*                        |
	| \\s*Extension:\\sNUnit.Engine.Drivers.NUnit2FrameworkDriver\\s*              |
	| \\s*Extension:\\sNUnit.Engine.Listeners.TeamCityEventListener\\s*            |
	| \\s*Extension:\\sNUnit.Engine.Services.ProjectLoaders.NUnitProjectLoader\\s* |

@3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages from SetUp and TearDown
	Given Framework version is <frameworkVersion>	
	And I have added SetUpWithOutput method as SetUpWithOutput to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added Successful method as SuccessfulTest1 to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added Successful method as SuccessfulTest2 to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added TearDownWithOutput method as TearDownWithOutput to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll	
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                                 | captureStandardOutput | duration | flowId | parent | message | details | out                                           | tc:tags                       |
	| flowStarted       |                                      |                       |          | .+     | .+     |         |         |                                               |                               |
	| testSuiteStarted  | foo.tests.dll                        |                       |          | .+     |        |         |         |                                               |                               |
	| flowStarted       |                                      |                       |          | .+     | .+     |         |         |                                               |                               |
	| testStarted       | Foo.Tests.UnitTests1.SuccessfulTest1 | false                 |          | .+     |        |         |         |                                               |                               |
	| testStdOut        | Foo.Tests.UnitTests1.SuccessfulTest1 |                       |          | .+     |        |         |         | SetUp output\|r\|noutput\|r\|nTearDown output | tc:parseServiceMessagesInside |
	| testFinished      | Foo.Tests.UnitTests1.SuccessfulTest1 |                       | \d+      | .+     |        |         |         |                                               |                               |
	| flowFinished      |                                      |                       |          | .+     |        |         |         |                                               |                               |
	| flowStarted       |                                      |                       |          | .+     | .+     |         |         |                                               |                               |
	| testStarted       | Foo.Tests.UnitTests1.SuccessfulTest2 | false                 |          | .+     |        |         |         |                                               |                               |
	| testStdOut        | Foo.Tests.UnitTests1.SuccessfulTest2 |                       |          | .+     |        |         |         | SetUp output\|r\|noutput\|r\|nTearDown output | tc:parseServiceMessagesInside |
	| testFinished      | Foo.Tests.UnitTests1.SuccessfulTest2 |                       | \d+      | .+     |        |         |         |                                               |                               |
	| flowFinished      |                                      |                       |          | .+     |        |         |         |                                               |                               |
	| testSuiteFinished | foo.tests.dll                        |                       |          | .+     |        |         |         |                                               |                               |
	| flowFinished      |                                      |                       |          | .+     |        |         |         |                                               |                               |
Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |

@3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run test scenarios for NUnit3
	Given Framework version is <frameworkVersion>	
	And I have added TestCasesWithSymbols method as TestCasesWithSymbols to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll	
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain 139 TeamCity service messages
Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |

@3.6.0
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run test with polish name for NUnit3
	Given Framework version is <frameworkVersion>
	And I have specified encoding <encoding>
	And I have added successful method as da_się_przefiltrować_produkty_dodatkowe_po_nazwie to the class Foo.Tests.UnitTests1 for foo.tests	
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll	
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	And I have added the arg Encoding=<encoding> to NUnit console command line
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                                                                   | captureStandardOutput | duration | flowId | parent | message | details | out    | tc:tags                       |
	| flowStarted       |                                                                        |                       |          | .+     | .+     |         |         |        |                               |
	| testSuiteStarted  | foo.tests.dll                                                          |                       |          | .+     |        |         |         |        |                               |
	| flowStarted       |                                                                        |                       |          | .+     | .+     |         |         |        |                               |
	| testStarted       | Foo.Tests.UnitTests1.da_się_przefiltrować_produkty_dodatkowe_po_nazwie | false                 |          | .+     |        |         |         |        |                               |
	| testStdOut        | Foo.Tests.UnitTests1.da_się_przefiltrować_produkty_dodatkowe_po_nazwie |                       |          | .+     |        |         |         | output | tc:parseServiceMessagesInside |
	| testFinished      | Foo.Tests.UnitTests1.da_się_przefiltrować_produkty_dodatkowe_po_nazwie |                       | \d+      | .+     |        |         |         |        |                               |
	| flowFinished      |                                                                        |                       |          | .+     |        |         |         |        |                               |
	| testSuiteFinished | foo.tests.dll                                                          |                       |          | .+     |        |         |         |        |                               |
	| flowFinished      |                                                                        |                       |          | .+     |        |         |         |        |                               |
Examples:
	| frameworkVersion | encoding     |
	| Version45        | utf-8        |
	| Version40        | utf-8        |
	| Version45        | windows-1250 |
	| Version40        | windows-1250 |


@3.6.0
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run test with russian output for NUnit3
	Given Framework version is <frameworkVersion>
	And I have specified encoding <encoding>
	And I have added RusTest method as RusTest to the class Foo.Tests.UnitTests1 for foo.tests	
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll	
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	And I have added the arg Encoding=<encoding> to NUnit console command line
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                         | captureStandardOutput | duration | flowId | parent | message | details | out    | tc:tags                       |
	| flowStarted       |                              |                       |          | .+     | .+     |         |         |        |                               |
	| testSuiteStarted  | foo.tests.dll                |                       |          | .+     |        |         |         |        |                               |
	| flowStarted       |                              |                       |          | .+     | .+     |         |         |        |                               |
	| testStarted       | Foo.Tests.UnitTests1.RusTest | false                 |          | .+     |        |         |         |        |                               |
	| testStdOut        | Foo.Tests.UnitTests1.RusTest |                       |          | .+     |        |         |         | привет | tc:parseServiceMessagesInside |
	| testFinished      | Foo.Tests.UnitTests1.RusTest |                       | \d+      | .+     |        |         |         |        |                               |
	| flowFinished      |                              |                       |          | .+     |        |         |         |        |                               |
	| testSuiteFinished | foo.tests.dll                |                       |          | .+     |        |         |         |        |                               |
	| flowFinished      |                              |                       |          | .+     |        |         |         |        |                               |
Examples:
	| frameworkVersion | encoding     |
	| Version45        | utf-8        |
	| Version40        | utf-8        |
	| Version45        | windows-1251 |
	| Version40        | windows-1251 |

@teamcity
Scenario Outline: NUnit sends TeamCity's service messages including stack trace when OneTimeSetUp in SetUpFixture throws exception
	Given Framework version is <frameworkVersion>
	And I have added successful method as Test1 to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added successful method as Test2 to the class Foo.Tests.UnitTests2 for foo.tests
	And I have added FailedOneTimeSetUp method as FailedOneTimeSetUp to the class Foo.Tests.FailingSetUpFixture for foo.tests
	And I have added attribute [NUnit.Framework.SetUpFixture] to the class Foo.Tests.FailingSetUpFixture for foo.tests
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	And I want to use CmdArguments configuration type
	And I have added the arg process=InProcess to NUnit console command line
	When I run NUnit console
	Then the exit code should be 2
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                       | captureStandardOutput | duration | flowId | parent | message                                             | details                                                | out | tc:tags |
	| flowStarted       |                            |                       |          | .+     | .+     |                                                     |                                                        |     |         |
	| testSuiteStarted  | foo.tests.dll              |                       |          | .+     |        |                                                     |                                                        |     |         |
	| testStarted       | Foo.Tests.UnitTests1.Test1 | false                 |          | .+     |        |                                                     |                                                        |     |         |
	| testFailed        | Foo.Tests.UnitTests1.Test1 |                       |          | .+     |        | System\.Exception : Exception during one time setup | at Foo\.Tests\.FailingSetUpFixture\.FailedOneTimeSetUp |     |         |
	| testFinished      | Foo.Tests.UnitTests1.Test1 |                       | 0        | .+     |        |                                                     |                                                        |     |         |
	| testStarted       | Foo.Tests.UnitTests2.Test2 | false                 |          | .+     |        |                                                     |                                                        |     |         |
	| testFailed        | Foo.Tests.UnitTests2.Test2 |                       |          | .+     |        | System\.Exception : Exception during one time setup | at Foo\.Tests\.FailingSetUpFixture\.FailedOneTimeSetUp |     |         |
	| testFinished      | Foo.Tests.UnitTests2.Test2 |                       | 0        | .+     |        |                                                     |                                                        |     |         |
	| testSuiteFinished | foo.tests.dll              |                       |          | .+     |        |                                                     |                                                        |     |         |
	| flowFinished      |                            |                       |          | .+     |        |                                                     |                                                        |     |         |
	Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |

@teamcity
Scenario Outline: NUnit sends TeamCity'successful s service messages when OneTimeTearDown in SetUpFixture throws exception
	Given Framework version is <frameworkVersion>
	And I have added successful method as Test1 to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added successful method as Test2 to the class Foo.Tests.UnitTests2 for foo.tests
	And I have added FailedOneTimeTearDown method as FailedOneTimeTearDown to the class Foo.Tests.FailingSetUpFixture for foo.tests
	And I have added attribute [NUnit.Framework.SetUpFixture] to the class Foo.Tests.FailingSetUpFixture for foo.tests
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	And I want to use CmdArguments configuration type
	And I have added the arg process=InProcess to NUnit console command line
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                       | captureStandardOutput | duration | flowId | parent | message | details | out    | tc:tags                       |
	| flowStarted       |                            |                       |          | .+     | .+     |         |         |        |                               |
	| testSuiteStarted  | foo.tests.dll              |                       |          | .+     |        |         |         |        |                               |
	| flowStarted       |                            |                       |          | .+     | .+     |         |         |        |                               |
	| testStarted       | Foo.Tests.UnitTests1.Test1 | false                 |          | .+     |        |         |         |        |                               |
	| testStdOut        | Foo.Tests.UnitTests1.Test1 |                       |          | .+     |        |         |         | output | tc:parseServiceMessagesInside |
	| testFinished      | Foo.Tests.UnitTests1.Test1 |                       | \d+      | .+     |        |         |         |        |                               |
	| flowFinished      |                            |                       |          | .+     |        |         |         |        |                               |
	| flowStarted       |                            |                       |          | .+     | .+     |         |         |        |                               |
	| testStarted       | Foo.Tests.UnitTests2.Test2 | false                 |          | .+     |        |         |         |        |                               |
	| testStdOut        | Foo.Tests.UnitTests2.Test2 |                       |          | .+     |        |         |         | output | tc:parseServiceMessagesInside |
	| testFinished      | Foo.Tests.UnitTests2.Test2 |                       | \d+      | .+     |        |         |         |        |                               |
	| flowFinished      |                            |                       |          | .+     |        |         |         |        |                               |
	| testSuiteFinished | foo.tests.dll              |                       |          | .+     |        |         |         |        |                               |
	| flowFinished      |                            |                       |          | .+     |        |         |         |        |                               |
	Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |

@teamcity
Scenario Outline: NUnit sends TeamCity's service messages including stack trace when TestFixtureSetup in SetUpFixture throws exception for NUnit2
	Given Framework version is <frameworkVersion>
	And I have added successful method as Test1 to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added successful method as Test2 to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added FailedTestFixtureSetup method as FailedTestFixtureSetup to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added attribute [NUnit.Framework.TestFixture] to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have copied the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to folder mocks
	And I have added the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to foo.tests
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	And I want to use CmdArguments configuration type
	And I have added the arg process=InProcess to NUnit console command line
	When I run NUnit console
	Then the exit code should be 2
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                       | captureStandardOutput | duration | flowId | parent | message                                               | details                                           | out | tc:tags |
	| testSuiteStarted  | foo.tests.dll              |                       |          | .+     |        |                                                       |                                                   |     |         |
	| testStarted       | Foo.Tests.UnitTests1.Test1 | false                 |          | .+     |        |                                                       |                                                   |     |         |
	| testFailed        | Foo.Tests.UnitTests1.Test1 |                       |          | .+     |        | System\.Exception : Exception during TestFixtureSetup | at Foo\.Tests\.UnitTests1\.FailedTestFixtureSetup |     |         |
	| testFinished      | Foo.Tests.UnitTests1.Test1 |                       | 0        | .+     |        |                                                       |                                                   |     |         |
	| testStarted       | Foo.Tests.UnitTests1.Test2 | false                 |          | .+     |        |                                                       |                                                   |     |         |
	| testFailed        | Foo.Tests.UnitTests1.Test2 |                       |          | .+     |        | System\.Exception : Exception during TestFixtureSetup | at Foo\.Tests\.UnitTests1\.FailedTestFixtureSetup |     |         |
	| testFinished      | Foo.Tests.UnitTests1.Test2 |                       | 0        | .+     |        |                                                       |                                                   |     |         |
	| testSuiteFinished | foo.tests.dll              |                       |          | .+     |        |                                                       |                                                   |     |         |
	Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |


    @teamcity
Scenario Outline: NUnit sends TeamCity'successful s service messages when FailedTestFixtureTearDown in SetUpFixture throws exception for NUnit2
	Given Framework version is <frameworkVersion>
	And I have added successful method as Test1 to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added successful method as Test2 to the class Foo.Tests.UnitTests2 for foo.tests
	And I have added FailedTestFixtureTearDown method as FailedTestFixtureTearDown to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added attribute [NUnit.Framework.TestFixture] to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have copied the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to folder mocks
	And I have added the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to foo.tests
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	And I want to use CmdArguments configuration type
	And I have added the arg process=InProcess to NUnit console command line
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                       | captureStandardOutput | duration | flowId | parent | message | details | out    | tc:tags                       |
	| testSuiteStarted  | foo.tests.dll              |                       |          | .+     |        |         |         |        |                               |
	| testStarted       | Foo.Tests.UnitTests1.Test1 | false                 |          | .+     |        |         |         |        |                               |
	| testStdOut        | Foo.Tests.UnitTests1.Test1 |                       |          | .+     |        |         |         | output | tc:parseServiceMessagesInside |
	| testFinished      | Foo.Tests.UnitTests1.Test1 |                       | \d+      | .+     |        |         |         |        |                               |
	| testStarted       | Foo.Tests.UnitTests2.Test2 | false                 |          | .+     |        |         |         |        |                               |
	| testStdOut        | Foo.Tests.UnitTests2.Test2 |                       |          | .+     |        |         |         | output | tc:parseServiceMessagesInside |
	| testFinished      | Foo.Tests.UnitTests2.Test2 |                       | \d+      | .+     |        |         |         |        |                               |
	| testSuiteFinished | foo.tests.dll              |                       |          | .+     |        |         |         |        |                               |
	Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |


@3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run it for one time setup with output
	Given Framework version is <frameworkVersion>	
	And I have added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added OneTimeSetUpWithOutput method as OneTimeSetUpWithOutput to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                                | captureStandardOutput | duration | flowId | parent | message             | details | out    |
	| flowStarted       |                                     |                       |          | .+     | .+     |                     |         |        |
	| testSuiteStarted  | foo.tests.dll                       |                       |          | .+     |        |                     |         |        |
	| flowStarted       |                                     |                       |          | .+     | .+     |                     |         |        |
	| testStarted       | Foo.Tests.UnitTests1.SuccessfulTest | false                 |          | .+     |        |                     |         |        |
	| testStdOut        | Foo.Tests.UnitTests1.SuccessfulTest |                       |          | .+     |        |                     |         | output |
	| testFinished      | Foo.Tests.UnitTests1.SuccessfulTest |                       | \d+      | .+     |        |                     |         |        |
	| flowFinished      |                                     |                       |          | .+     |        |                     |         |        |
	| message           |                                     |                       |          | .+     |        | OneTimeSetUp output |         |        |
	| testSuiteFinished | foo.tests.dll                       |                       |          | .+     |        |                     |         |        |
	| flowFinished      |                                     |                       |          | .+     |        |                     |         |        |
Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |


    @3.4.1
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run it for one time TearDown with output
	Given Framework version is <frameworkVersion>	
	And I have added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added OneTimeTearDownWithOutput method as OneTimeTearDownWithOutput to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                                | captureStandardOutput | duration | flowId | parent | message                | details | out    |
	| flowStarted       |                                     |                       |          | .+     | .+     |                        |         |        |
	| testSuiteStarted  | foo.tests.dll                       |                       |          | .+     |        |                        |         |        |
	| flowStarted       |                                     |                       |          | .+     | .+     |                        |         |        |
	| testStarted       | Foo.Tests.UnitTests1.SuccessfulTest | false                 |          | .+     |        |                        |         |        |
	| testStdOut        | Foo.Tests.UnitTests1.SuccessfulTest |                       |          | .+     |        |                        |         | output |
	| testFinished      | Foo.Tests.UnitTests1.SuccessfulTest |                       | \d+      | .+     |        |                        |         |        |
	| flowFinished      |                                     |                       |          | .+     |        |                        |         |        |
	| message           |                                     |                       |          | .+     |        | OneTimeTearDown output |         |        |
	| testSuiteFinished | foo.tests.dll                       |                       |          | .+     |        |                        |         |        |
	| flowFinished      |                                     |                       |          | .+     |        |                        |         |        |
Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |

@teamcity
Scenario Outline: NUnit sends TeamCity's service messages with output when TestFixtureSetup in SetUpFixture
	Given Framework version is <frameworkVersion>
	And I have added successful method as Test1 to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added successful method as Test2 to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added TestFixtureSetupWithOutput method as TestFixtureSetupWithOutput to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added attribute [NUnit.Framework.TestFixture] to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have copied the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to folder mocks
	And I have added the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to foo.tests
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	And I want to use CmdArguments configuration type
	And I have added the arg process=InProcess to NUnit console command line
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                       | captureStandardOutput | duration | flowId | parent | message | details | out                             | tc:tags |
	| flowStarted       |                            |                       |          | .+     | .+     |         |         |                                 |         |
	| testSuiteStarted  | foo.tests.dll              |                       |          | .+     |        |         |         |                                 |         |
	| testStarted       | Foo.Tests.UnitTests1.Test1 | false                 |          | .+     |        |         |         |                                 |         |
	| testStdOut        | Foo.Tests.UnitTests1.Test1 |                       |          | .+     |        |         |         | TestFixtureSetup output.+output |         |
	| testFinished      | Foo.Tests.UnitTests1.Test1 |                       |          | .+     |        |         |         |                                 |         |
	| testStarted       | Foo.Tests.UnitTests1.Test2 | false                 |          | .+     |        |         |         |                                 |         |
	| testStdOut        | Foo.Tests.UnitTests1.Test2 |                       |          | .+     |        |         |         | output                          |         |
	| testFinished      | Foo.Tests.UnitTests1.Test2 |                       |          | .+     |        |         |         |                                 |         |
	| testSuiteFinished | foo.tests.dll              |                       |          | .+     |        |         |         |                                 |         |
	| flowFinished      |                            |                       |          | .+     |        |         |         |                                 |         |
	Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |

@teamcity
Scenario Outline: NUnit sends TeamCity's service messages with output when TestFixtureTearDown in SetUpFixture
	Given Framework version is <frameworkVersion>
	And I have added successful method as Test1 to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added successful method as Test2 to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added TestFixtureTearDownWithOutput method as TestFixtureTearDownWithOutput to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added attribute [NUnit.Framework.TestFixture] to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have copied the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to folder mocks
	And I have added the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to foo.tests
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	And I want to use CmdArguments configuration type
	And I have added the arg process=InProcess to NUnit console command line
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                       | captureStandardOutput | duration | flowId | parent | message                    | details | out    | tc:tags |
	| flowStarted       |                            |                       |          | .+     | .+     |                            |         |        |         |
	| testSuiteStarted  | foo.tests.dll              |                       |          | .+     |        |                            |         |        |         |
	| testStarted       | Foo.Tests.UnitTests1.Test1 | false                 |          | .+     |        |                            |         |        |         |
	| testStdOut        | Foo.Tests.UnitTests1.Test1 |                       |          | .+     |        |                            |         | output |         |
	| testFinished      | Foo.Tests.UnitTests1.Test1 |                       |          | .+     |        |                            |         |        |         |
	| testStarted       | Foo.Tests.UnitTests1.Test2 | false                 |          | .+     |        |                            |         |        |         |
	| testStdOut        | Foo.Tests.UnitTests1.Test2 |                       |          | .+     |        |                            |         | output |         |
	| testFinished      | Foo.Tests.UnitTests1.Test2 |                       |          | .+     |        |                            |         |        |         |
	| message           |                            |                       |          | .+     |        | TestFixtureTearDown output |         |        |         |
	| testSuiteFinished | foo.tests.dll              |                       |          | .+     |        |                            |         |        |         |
	| flowFinished      |                            |                       |          | .+     |        |                            |         |        |         |
	Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |

@3.9
@teamcity
Scenario Outline: NUnit sends TeamCity's service messages when I run successful test writing to stdError
	Given Framework version is <frameworkVersion>
	And I have added SuccessfulStdError method as SuccessfulStdErrorTest to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the output should contain TeamCity service messages:
	|                   | name                                        | captureStandardOutput | duration | flowId | parent | message | details | out      | tc:tags                       |
	| flowStarted       |                                             |                       |          | .+     | .+     |         |         |          |                               |
	| testSuiteStarted  | foo.tests.dll                               |                       |          | .+     |        |         |         |          |                               |
	| flowStarted       |                                             |                       |          | .+     | .+     |         |         |          |                               |
	| testStarted       | Foo.Tests.UnitTests1.SuccessfulStdErrorTest | false                 |          | .+     |        |         |         |          |                               |
	| testStdErr        | Foo.Tests.UnitTests1.SuccessfulStdErrorTest |                       |          | .+     |        |         |         | errorout | tc:parseServiceMessagesInside |
	| testStdOut        | Foo.Tests.UnitTests1.SuccessfulStdErrorTest |                       |          | .+     |        |         |         | stdout   | tc:parseServiceMessagesInside |
	| testFinished      | Foo.Tests.UnitTests1.SuccessfulStdErrorTest |                       | \d+      | .+     |        |         |         |          |                               |
	| flowFinished      |                                             |                       |          | .+     |        |         |         |          |                               |
	| testSuiteFinished | foo.tests.dll                               |                       |          | .+     |        |         |         |          |                               |
	| flowFinished      |                                             |                       |          | .+     |        |         |         |          |                               |
Examples:
	| frameworkVersion |
	| Version45        |
	| Version40        |
