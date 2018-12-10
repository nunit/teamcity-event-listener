Feature: User runs tests

Background:
	Given NUnit path is ..\nunit\

@3.4.1
Scenario Outline: User runs tests for several assemblies
	Given Framework version is <frameworkVersion>
	And I have created the folder mocks
	And I have copied NUnit framework references to folder mocks
	And I have added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests	
	And I have added NUnit framework references to foo.tests	
	
	And I have compiled the assembly foo.tests to file mocks\foo1.tests.dll
	And I have added the assembly mocks\foo1.tests.dll to the list of testing assemblies

	And I have compiled the assembly foo.tests to file mocks\foo2.tests.dll
	And I have added the assembly mocks\foo2.tests.dll to the list of testing assemblies

	And I have compiled the assembly foo.tests to file mocks\foo3.tests.dll
	And I have added the assembly mocks\foo3.tests.dll to the list of testing assemblies

	And I have compiled the assembly foo.tests to file mocks\foo4.tests.dll
	And I have added the assembly mocks\foo4.tests.dll to the list of testing assemblies

	And I have compiled the assembly foo.tests to file mocks\foo5.tests.dll
	And I have added the assembly mocks\foo5.tests.dll to the list of testing assemblies

	And I have compiled the assembly foo.tests to file mocks\foo6.tests.dll
	And I have added the assembly mocks\foo6.tests.dll to the list of testing assemblies
	
	And I want to use <configurationType> configuration type
	And I have added the arg workers=10 to NUnit console command line
	And I have added the arg agents=<agents> to NUnit console command line
	And I have added the arg process=<process> to NUnit console command line
	And I have added the arg domain=<domain> to NUnit console command line

	When I run NUnit console
	And the output should contain correct set of TeamCity service messages
	And the Test Run Summary should has following:
	| field        | value |
	| Test Count   | 6     |
	| Passed       | 6     |
	| Failed       | 0     |
	| Inconclusive | 0     |
	| Skipped      | 0     |
Examples:
	| frameworkVersion | process   | domain   | agents | platform | configurationType |
	| Version40        | InProcess | Single   | 2      | AnyCpu   | ProjectFile       |
	| Version45        | Separate  | Single   | 2      | AnyCpu   | ProjectFile       |
	| Version40        | Separate  | Single   | 2      | AnyCpu   | ProjectFile       |
	| Version45        | Multiple  | Single   | 2      | AnyCpu   | ProjectFile       |
	| Version40        | Multiple  | Single   | 2      | AnyCpu   | ProjectFile       |
	| Version45        | InProcess | Multiple | 2      | AnyCpu   | ProjectFile       |
	| Version40        | InProcess | Multiple | 2      | AnyCpu   | ProjectFile       |
	| Version45        | Separate  | Multiple | 2      | AnyCpu   | ProjectFile       |
	| Version40        | Separate  | Multiple | 2      | AnyCpu   | ProjectFile       |
	| Version45        | InProcess | Single   | 1      | AnyCpu   | ProjectFile       |
	| Version40        | InProcess | Single   | 1      | AnyCpu   | ProjectFile       |
	| Version45        | Separate  | Single   | 1      | AnyCpu   | ProjectFile       |
	| Version40        | Separate  | Single   | 1      | AnyCpu   | ProjectFile       |
	| Version45        | Multiple  | Single   | 1      | AnyCpu   | ProjectFile       |
	| Version40        | Multiple  | Single   | 1      | AnyCpu   | ProjectFile       |
	| Version45        | InProcess | Multiple | 1      | AnyCpu   | ProjectFile       |
	| Version40        | InProcess | Multiple | 1      | AnyCpu   | ProjectFile       |
	| Version45        | Separate  | Multiple | 1      | AnyCpu   | ProjectFile       |
	| Version40        | Separate  | Multiple | 1      | AnyCpu   | ProjectFile       |
	| Version45        | Separate  | Single   | 1      | X86      | ProjectFile       |
	| Version40        | Separate  | Single   | 1      | X86      | ProjectFile       |
	| Version45        | Multiple  | Single   | 1      | X86      | ProjectFile       |
	| Version40        | Multiple  | Single   | 1      | X86      | ProjectFile       |
	| Version45        | Separate  | Multiple | 1      | X86      | ProjectFile       |
	| Version40        | Separate  | Multiple | 1      | X86      | ProjectFile       |
	| Version40        | InProcess | Single   | 2      | X86      | ProjectFile       |
	| Version45        | Separate  | Single   | 2      | X86      | ProjectFile       |
	| Version40        | Separate  | Single   | 2      | X86      | ProjectFile       |
	| Version45        | Multiple  | Single   | 2      | X86      | ProjectFile       |
	| Version40        | Multiple  | Single   | 2      | X86      | ProjectFile       |
	| Version45        | InProcess | Multiple | 2      | X86      | ProjectFile       |
	| Version40        | InProcess | Multiple | 2      | X86      | ProjectFile       |
	| Version45        | Separate  | Multiple | 2      | X86      | ProjectFile       |
	| Version40        | Separate  | Multiple | 2      | X86      | ProjectFile       |
	| Version40        | InProcess | Single   | 2      | AnyCpu   | CmdArguments      |
	| Version45        | Separate  | Single   | 2      | AnyCpu   | CmdArguments      |
	| Version40        | Separate  | Single   | 2      | AnyCpu   | CmdArguments      |
	| Version45        | Multiple  | Single   | 2      | AnyCpu   | CmdArguments      |
	| Version40        | Multiple  | Single   | 2      | AnyCpu   | CmdArguments      |
	| Version45        | InProcess | Multiple | 2      | AnyCpu   | CmdArguments      |
	| Version40        | InProcess | Multiple | 2      | AnyCpu   | CmdArguments      |
	| Version45        | Separate  | Multiple | 2      | AnyCpu   | CmdArguments      |
	| Version40        | Separate  | Multiple | 2      | AnyCpu   | CmdArguments      |
	| Version45        | InProcess | Single   | 1      | AnyCpu   | CmdArguments      |
	| Version40        | InProcess | Single   | 1      | AnyCpu   | CmdArguments      |
	| Version45        | Separate  | Single   | 1      | AnyCpu   | CmdArguments      |
	| Version40        | Separate  | Single   | 1      | AnyCpu   | CmdArguments      |
	| Version45        | Multiple  | Single   | 1      | AnyCpu   | CmdArguments      |
	| Version40        | Multiple  | Single   | 1      | AnyCpu   | CmdArguments      |
	| Version45        | InProcess | Multiple | 1      | AnyCpu   | CmdArguments      |
	| Version40        | InProcess | Multiple | 1      | AnyCpu   | CmdArguments      |
	| Version45        | Separate  | Multiple | 1      | AnyCpu   | CmdArguments      |
	| Version40        | Separate  | Multiple | 1      | AnyCpu   | CmdArguments      |
	| Version45        | Separate  | Single   | 1      | X86      | CmdArguments      |
	| Version40        | Separate  | Single   | 1      | X86      | CmdArguments      |
	| Version45        | Multiple  | Single   | 1      | X86      | CmdArguments      |
	| Version40        | Multiple  | Single   | 1      | X86      | CmdArguments      |
	| Version45        | Separate  | Multiple | 1      | X86      | CmdArguments      |
	| Version40        | Separate  | Multiple | 1      | X86      | CmdArguments      |
	| Version40        | InProcess | Single   | 2      | X86      | CmdArguments      |
	| Version45        | Separate  | Single   | 2      | X86      | CmdArguments      |
	| Version40        | Separate  | Single   | 2      | X86      | CmdArguments      |
	| Version45        | Multiple  | Single   | 2      | X86      | CmdArguments      |
	| Version40        | Multiple  | Single   | 2      | X86      | CmdArguments      |
	| Version45        | InProcess | Multiple | 2      | X86      | CmdArguments      |
	| Version40        | InProcess | Multiple | 2      | X86      | CmdArguments      |
	| Version45        | Separate  | Multiple | 2      | X86      | CmdArguments      |
	| Version40        | Separate  | Multiple | 2      | X86      | CmdArguments      |

@3.4.1
Scenario Outline: User runs parallelizable tests
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
	And I have specified <platform> platform for assembly foo1.tests
	And I have compiled the assembly foo1.tests to file mocks\foo1.tests.dll	
	And I have specified <platform> platform for assembly foo2.tests
	And I have compiled the assembly foo2.tests to file mocks\foo2.tests.dll	
	And I have added the assembly mocks\foo1.tests.dll to the list of testing assemblies
	And I have added the assembly mocks\foo2.tests.dll to the list of testing assemblies
	And I want to use CmdArguments type of TeamCity integration
	And I have added the arg workers=10 to NUnit console command line
	And I have added the arg agents=<agents> to NUnit console command line
	And I have added the arg process=<process> to NUnit console command line
	And I have added the arg domain=<domain> to NUnit console command line
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the Test Run Summary should has following:
	| field        | value |
	| Test Count   | 6     |
	| Passed       | 6     |
	| Failed       | 0     |
	| Inconclusive | 0     |
	| Skipped      | 0     |
Examples:
	| frameworkVersion | process   | domain   | agents | platform |
	| Version45        | InProcess | Single   | 10     | AnyCpu   |
	| Version40        | InProcess | Single   | 10     | AnyCpu   |
	| Version45        | Separate  | Single   | 10     | AnyCpu   |
	| Version40        | Separate  | Single   | 10     | AnyCpu   |
	| Version45        | Multiple  | Single   | 10     | AnyCpu   |
	| Version40        | Multiple  | Single   | 10     | AnyCpu   |
	| Version45        | InProcess | Multiple | 10     | AnyCpu   |
	| Version40        | InProcess | Multiple | 10     | AnyCpu   |
	| Version45        | Separate  | Multiple | 10     | AnyCpu   |
	| Version40        | Separate  | Multiple | 10     | AnyCpu   |
	| Version45        | InProcess | Single   | 1      | AnyCpu   |
	| Version40        | InProcess | Single   | 1      | AnyCpu   |
	| Version45        | Separate  | Single   | 1      | AnyCpu   |
	| Version40        | Separate  | Single   | 1      | AnyCpu   |
	| Version45        | Multiple  | Single   | 1      | AnyCpu   |
	| Version40        | Multiple  | Single   | 1      | AnyCpu   |
	| Version45        | InProcess | Multiple | 1      | AnyCpu   |
	| Version40        | InProcess | Multiple | 1      | AnyCpu   |
	| Version45        | Separate  | Multiple | 1      | AnyCpu   |
	| Version40        | Separate  | Multiple | 1      | AnyCpu   |
	| Version45        | Separate  | Single   | 10     | X86     |
	| Version40        | Separate  | Single   | 10     | X86     |
	| Version45        | Multiple  | Single   | 10     | X86     |
	| Version40        | Multiple  | Single   | 10     | X86     |
	| Version45        | Separate  | Multiple | 10     | X86     |
	| Version40        | Separate  | Multiple | 10     | X86     |
	| Version45        | Separate  | Single   | 1      | X86     |
	| Version40        | Separate  | Single   | 1      | X86     |
	| Version45        | Multiple  | Single   | 1      | X86     |
	| Version40        | Multiple  | Single   | 1      | X86     |
	| Version45        | Separate  | Multiple | 1      | X86     |
	| Version40        | Separate  | Multiple | 1      | X86     |

@3.4.1
Scenario Outline: User runs parallelizable tests for NUnit 2 framework
	And I have added successful method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests
	And I have added successfulCatA method as SuccessfulTestCatA to the class Foo.Tests.UnitTests1 for foo.tests	
	And I have created the folder mocks
	And I have added the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to foo.tests
	And I have copied the reference ..\..\packages\NUnit.2.6.4\lib\nunit.framework.dll to folder mocks
	And I have specified <platform> platform for assembly foo.tests
	And I have compiled the assembly foo.tests to file mocks\foo.tests.dll	
	And I have added the assembly mocks\foo.tests.dll to the list of testing assemblies
	And I have added the arg Where=cat!=CatA to NUnit console command line
	And I want to use CmdArguments type of TeamCity integration
	And I have added the arg workers=10 to NUnit console command line
	And I have added the arg agents=<agents> to NUnit console command line
	And I have added the arg process=<process> to NUnit console command line
	And I have added the arg domain=<domain> to NUnit console command line
	When I run NUnit console
	Then the exit code should be 0
	And the output should contain correct set of TeamCity service messages
	And the Test Run Summary should has following:
	| field        | value |
	| Test Count   | 1     |
	| Passed       | 1     |
	| Failed       | 0     |
	| Inconclusive | 0     |
	| Skipped      | 0     |
Examples:
	| frameworkVersion | process   | domain   | agents | platform |
	| Version45        | InProcess | Single   | 10     | AnyCpu   |
	| Version40        | InProcess | Single   | 10     | AnyCpu   |
	| Version45        | Separate  | Single   | 10     | AnyCpu   |
	| Version40        | Separate  | Single   | 10     | AnyCpu   |
	| Version45        | Multiple  | Single   | 10     | AnyCpu   |
	| Version40        | Multiple  | Single   | 10     | AnyCpu   |
	| Version45        | InProcess | Multiple | 10     | AnyCpu   |
	| Version40        | InProcess | Multiple | 10     | AnyCpu   |
	| Version45        | Separate  | Multiple | 10     | AnyCpu   |
	| Version40        | Separate  | Multiple | 10     | AnyCpu   |
#	| Version45        | Multiple  | Multiple | 10     | AnyCpu   |
#	| Version40        | Multiple  | Multiple | 10     | AnyCpu   |
	| Version45        | InProcess | Single   | 1      | AnyCpu   |
	| Version40        | InProcess | Single   | 1      | AnyCpu   |
	| Version45        | Separate  | Single   | 1      | AnyCpu   |
	| Version40        | Separate  | Single   | 1      | AnyCpu   |
	| Version45        | Multiple  | Single   | 1      | AnyCpu   |
	| Version40        | Multiple  | Single   | 1      | AnyCpu   |
	| Version45        | InProcess | Multiple | 1      | AnyCpu   |
	| Version40        | InProcess | Multiple | 1      | AnyCpu   |
	| Version45        | Separate  | Multiple | 1      | AnyCpu   |
	| Version40        | Separate  | Multiple | 1      | AnyCpu   |
	| Version45        | Separate  | Single   | 10     | X86      |
	| Version40        | Separate  | Single   | 10     | X86      |
	| Version45        | Multiple  | Single   | 10     | X86      |
	| Version40        | Multiple  | Single   | 10     | X86      |
	| Version45        | Separate  | Multiple | 10     | X86      |
	| Version40        | Separate  | Multiple | 10     | X86      |
	| Version45        | Separate  | Single   | 1      | X86      |
	| Version40        | Separate  | Single   | 1      | X86      |
	| Version45        | Multiple  | Single   | 1      | X86      |
	| Version40        | Multiple  | Single   | 1      | X86      |
	| Version45        | Separate  | Multiple | 1      | X86      |
	| Version40        | Separate  | Multiple | 1      | X86      |