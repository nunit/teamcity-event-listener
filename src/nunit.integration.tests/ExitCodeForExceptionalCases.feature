Feature: NUnit should returns negative exit code in the exceptional cases

Background:
	Given NUnit path is ..\nunit\

@3.4.1
@teamcity
Scenario Outline: NUnit returns negative exit -2 code when I run it without any assemblies
	Given Framework version is <frameworkVersion>	
	And I have added the assembly mocks\notInTheDir.dll to the list of testing assemblies
	And I have added the arg Process=<process> to NUnit console command line
	When I run NUnit console
	Then the exit code should be -2
Examples:
	| frameworkVersion | process   |
	| Version45        | InProcess |
	| Version40        | InProcess |
	| Version45        | Separate  |
	| Version40        | Separate  |
	| Version45        | Multiple  |
	| Version40        | Multiple  |

@3.4.1
@ignore
@teamcity
Scenario Outline: NUnit returns negative exit -2 code when I run it without nunit.framework.dll
	Given Framework version is <frameworkVersion>	
	And I have added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll	
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I have added the arg Process=<process> to NUnit console command line
	When I run NUnit console
	Then the exit code should be -2
Examples:
	| frameworkVersion | process   |
	| Version45        | InProcess |
	| Version40        | InProcess |
	| Version45        | Separate  |
	| Version40        | Separate  |
	| Version45        | Multiple  |
	| Version40        | Multiple  |
	
@3.9
@teamcity
Scenario Outline: NUnit returns exit code -5 when the test throws StackOverflow exception
	Given Framework version is <frameworkVersion>	
	And I have added failedStackOverflow method as FailedStackOverflow to the class Foo.Tests.UnitTests1 for foo.tests	
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll	
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	And I have added the arg Process=<process> to NUnit console command line
	When I run NUnit console
	Then the exit code should be -5
Examples:
	| frameworkVersion | process   |
#	| Version45        | InProcess |
#	| Version40        | InProcess |
	| Version45        | Separate  |
	| Version40        | Separate  |
	| Version45        | Multiple  |
	| Version40        | Multiple  |

@3.4.1
@teamcity
@ignore
Scenario Outline: NUnit returns positive exit code when the test throws OutOfMemory exception
	Given Framework version is <frameworkVersion>	
	And I have added failedOutOfMemory method as FailedOutOfMemory to the class Foo.Tests.UnitTests1 for foo.tests	
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll	
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	And I have added the arg Process=<process> to NUnit console command line
	When I run NUnit console
	Then the exit code should be 1
	And the Test Run Summary should has following:
	| field        | value |
	| Test Count   | 1     |
	| Passed       | 0     |
	| Failed       | 1     |
	| Inconclusive | 0     |
	| Skipped      | 0     |
Examples:
	| frameworkVersion | process   |
	| Version45        | InProcess |
	| Version40        | InProcess |
	| Version45        | Separate  |
	| Version40        | Separate  |
	| Version45        | Multiple  |
	| Version40        | Multiple  |