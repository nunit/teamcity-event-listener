Feature: TeamCity attachments

Background:
    Given NUnit path is ..\nunit\

@3.9
@teamcity
Scenario Outline: User can attach artifacts and test metadata to test
    Given Framework version is Version45
    And I added SuccessfulWithAttachedFiles method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests
    And I created the folder mocks
    And I added NUnit framework references to foo.tests
    And I copied NUnit framework references to folder mocks
    And I compiled the assembly foo.tests to file mocks\foo.tests.dll
    And I added the assembly mocks\foo.tests.dll to the list of testing assemblies
    And I want to use CmdArguments type of TeamCity integration
    And I added the environment variable TEAMCITY_VERSION as <teamCityVersion>
    And I appended the string MyImage to file Data\MyImage.jpg
    And I appended the string MyImage2 to file Data\MyImage2.gif
    And I appended the string Class to file Data\Class.cs
    And I appended the string report.txt to file Data\report.txt
    When I run NUnit console
    Then the exit code should be 0
    And the output should contain correct set of TeamCity service messages
    And the output should contain TeamCity service messages:
    |                   | .                        | name                                | testName                            | type     | value          | name     | flowId |
    | flowStarted       |                          |                                     |                                     |          |                |          | .+     |
    | testSuiteStarted  |                          | foo.tests.dll                       |                                     |          |                |          | .+     |
    | flowStarted       |                          |                                     |                                     |          |                |          | .+     |
    | testStarted       |                          | Foo.Tests.UnitTests1.SuccessfulTest |                                     |          |                |          | .+     |
    | publishArtifacts  | .+MyImage.jpg\\s=>\\s.+  |                                     |                                     |          |                |          |        |
    | testMetadata      |                          |                                     | Foo.Tests.UnitTests1.SuccessfulTest | image    | .+MyImage.jpg  | My Image | .+     |
    | publishArtifacts  | .+MyImage2.gif\\s=>\\s.+ |                                     |                                     |          |                |          |        |
    | testMetadata      |                          |                                     | Foo.Tests.UnitTests1.SuccessfulTest | image    | .+MyImage2.gif |          | .+     |
    | publishArtifacts  | .+Class.cs\\s=>\\s.+     |                                     |                                     |          |                |          |        |
    | testMetadata      |                          |                                     | Foo.Tests.UnitTests1.SuccessfulTest | artifact | .+Class.cs     | source   | .+     |
    | publishArtifacts  | .+report.txt\\s=>\\s.+   |                                     |                                     |          |                |          |        |
    | testMetadata      |                          |                                     | Foo.Tests.UnitTests1.SuccessfulTest | artifact | .+report.txt   |          | .+     |
    | testFinished      |                          | Foo.Tests.UnitTests1.SuccessfulTest |                                     |          |                |          | .+     |
    | flowFinished      |                          |                                     |                                     |          |                |          | .+     |
    | testSuiteFinished |                          | foo.tests.dll                       |                                     |          |                |          | .+     |
    | flowFinished      |                          |                                     |                                     |          |                |          | .+     |
Examples:
    | teamCityVersion         |
    | 2018.2 (build SNAPSHOT) |
    | 2018.2                  |
    | 2019                    |
    | 2019.2                    |

@3.9
@teamcity
Scenario: User can attach artifacts and test metadata using custom path
    Given Framework version is Version45
    And I added SuccessfulWithAttachedFileToCustomPath method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests
    And I created the folder mocks
    And I added NUnit framework references to foo.tests
    And I copied NUnit framework references to folder mocks
    And I compiled the assembly foo.tests to file mocks\foo.tests.dll
    And I added the assembly mocks\foo.tests.dll to the list of testing assemblies
    And I want to use CmdArguments type of TeamCity integration
    And I added the environment variable TEAMCITY_VERSION as 2018.2
    And I appended the string MyImage to file Data\MyImage.jpg
    And I appended the string report.txt to file Data\report.txt
    When I run NUnit console
    Then the exit code should be 0
    And the output should contain correct set of TeamCity service messages
    And the output should contain TeamCity service messages:
    |                   | .                       | name                                | testName                            | type     | value              | name     | flowId |
    | flowStarted       |                         |                                     |                                     |          |                    |          | .+     |
    | testSuiteStarted  |                         | foo.tests.dll                       |                                     |          |                    |          | .+     |
    | flowStarted       |                         |                                     |                                     |          |                    |          | .+     |
    | testStarted       |                         | Foo.Tests.UnitTests1.SuccessfulTest |                                     |          |                    |          | .+     |
    | publishArtifacts  | .+MyImage.jpg => images |                                     |                                     |          |                    |          |        |
    | testMetadata      |                         |                                     | Foo.Tests.UnitTests1.SuccessfulTest | image    | images/MyImage.jpg | My Image | .+     |
    | publishArtifacts  | .+report.txt => reports |                                     |                                     |          |                    |          |        |
    | testMetadata      |                         |                                     | Foo.Tests.UnitTests1.SuccessfulTest | artifact | reports/report.txt |          | .+     |
    | testFinished      |                         | Foo.Tests.UnitTests1.SuccessfulTest |                                     |          |                    |          | .+     |
    | flowFinished      |                         |                                     |                                     |          |                    |          | .+     |
    | testSuiteFinished |                         | foo.tests.dll                       |                                     |          |                    |          | .+     |
    | flowFinished      |                         |                                     |                                     |          |                    |          | .+     |

@3.9
@teamcity
Scenario Outline: Attachments should be suppressed
    Given Framework version is Version45
    And I added SuccessfulWithAttachedFiles method as SuccessfulTest to the class Foo.Tests.UnitTests1 for foo.tests
    And I created the folder mocks
    And I added NUnit framework references to foo.tests
    And I copied NUnit framework references to folder mocks
    And I compiled the assembly foo.tests to file mocks\foo.tests.dll
    And I added the assembly mocks\foo.tests.dll to the list of testing assemblies
    And I want to use CmdArguments type of TeamCity integration
    And I added the environment variable TEAMCITY_LOGGER_ALLOW_EXPERIMENTAL as <teamcityloggerExperimental>
    And I added the environment variable TEAMCITY_DOTNET_TEST_METADATA_ENABLE as <teamcityDotnetTestMetadataEnable>
    And I added the environment variable TEAMCITY_VERSION as <teamCityVersion>
    And I appended the string MyImage to file Data\MyImage.jpg
    And I appended the string MyImage2 to file Data\MyImage2.gif
    And I appended the string Class to file Data\Class.cs
    And I appended the string report.txt to file Data\report.txt
    When I run NUnit console
    Then the exit code should be 0
    And the output should contain correct set of TeamCity service messages
    And the output should contain TeamCity service messages:
    |                   | .                        | name                                | testName                            | type     | value          | name     | flowId |
    | flowStarted       |                          |                                     |                                     |          |                |          | .+     |
    | testSuiteStarted  |                          | foo.tests.dll                       |                                     |          |                |          | .+     |
    | flowStarted       |                          |                                     |                                     |          |                |          | .+     |
    | testStarted       |                          | Foo.Tests.UnitTests1.SuccessfulTest |                                     |          |                |          | .+     |
    | testFinished      |                          | Foo.Tests.UnitTests1.SuccessfulTest |                                     |          |                |          | .+     |
    | flowFinished      |                          |                                     |                                     |          |                |          | .+     |
    | testSuiteFinished |                          | foo.tests.dll                       |                                     |          |                |          | .+     |
    | flowFinished      |                          |                                     |                                     |          |                |          | .+     |
Examples:
    | teamCityVersion         | teamcityloggerExperimental | teamcityDotnetTestMetadataEnable |
    | 10.2                    | false                      | true                             |
    | 2018.1 (build SNAPSHOT) | false                      | true                             |
    | 2018.2 (build SNAPSHOT) | false                      | true                             |
    | 2018.2                  | false                      | true                             |
    | 2019                    | false                      | true                             |
    | 2018.2                  | False                      | true                             |
    | 2018.2                  | FALSE                      | true                             |
    | 10.2                    | true                       | false                            |
    | 2018.1 (build SNAPSHOT) | true                       | false                            |
    | 2018.2 (build SNAPSHOT) | true                       | false                            |
    | 2018.2                  | true                       | false                            |
    | 2019                    | true                       | false                            |
    | 2018.2                  | true                       | False                            |
    | 2018.2                  | true                       | FALSE                            |
    | 10.2                    | true                       | true                             |
    | 10                      | true                       | true                             |
    | 2018.1 (build SNAPSHOT) | true                       | true                             |
    | 2018.1 RC               | true                       | true                             |
    | 2018.1                  | true                       | true                             |
