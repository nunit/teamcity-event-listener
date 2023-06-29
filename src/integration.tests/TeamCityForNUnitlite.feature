Feature: NUnit should support TeamCity in NUnitlite

Background:
	Given NUnit path is ..\nunit\

@teamcity
Scenario Outline: NUnitlite sends TeamCity's service messages when I run successful test
	Given Framework version is <frameworkVersion>	
	And I have added NUnitLiteEntryPoint method as Main to the class Foo.Tests.Program for foo.tests	
	And I have added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests	
	And I have created the folder mocks
	And I have added NUnit framework references to foo.tests
	And I have added the reference mocks\nunitlite.dll to foo.tests
	And I have copied NUnit framework references to folder mocks
	And I have compiled the assembly foo.tests to file mocks\foo.tests.exe
	And I want to use <teamCityIntegration> type of TeamCity integration
	When I run mocks\foo.tests.exe
	Then the exit code should be 0
	And the output should contain TeamCity service messages:
	|                   | name                                | captureStandardOutput | duration | flowId | parent | message | details | out    | tc:tags                       |
	| testSuiteStarted  | foo.tests.dll                       |                       |          | .+     |        |         |         |        |                               |
	| flowStarted       |                                     |                       |          | .+     | .+     |         |         |        |                               |
	| testStarted       | Foo.Tests.UnitTests1.SuccessfulTest | false                 |          | .+     |        |         |         |        |                               |
	| testStdOut        | Foo.Tests.UnitTests1.SuccessfulTest |                       |          | .+     |        |         |         | output | tc:parseServiceMessagesInside |
	| testFinished      | Foo.Tests.UnitTests1.SuccessfulTest |                       | \d+      | .+     |        |         |         |        |                               |
	| flowFinished      |                                     |                       |          | .+     |        |         |         |        |                               |
	| testSuiteFinished | foo.tests.dll                       |                       |          | .+     |        |         |         |        |                               |

Examples:
	| frameworkVersion | teamCityIntegration |
	| Version45        | CmdArguments        |
	| Version40        | CmdArguments        |
	| Version45        | EnvVariable         |
	| Version40        | EnvVariable         |