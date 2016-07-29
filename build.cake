#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.1

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// SET PACKAGE VERSION
//////////////////////////////////////////////////////////////////////

var version = "1.0.2";
var modifier = "";

var dbgSuffix = configuration == "Debug" ? "-dbg" : "";
var packageVersion = version + modifier + dbgSuffix;

if (BuildSystem.IsRunningOnAppVeyor)
{
	var tag = AppVeyor.Environment.Repository.Tag;

	if (tag.IsTag)
	{
		packageVersion = tag.Name;
	}
	else
	{
		var buildNumber = AppVeyor.Environment.Build.Number;
		packageVersion = version + "-CI-" + buildNumber + dbgSuffix;
		if (AppVeyor.Environment.PullRequest.IsPullRequest)
			packageVersion += "-PR-" + AppVeyor.Environment.PullRequest.Number;
		else
			packageVersion += "-" + AppVeyor.Environment.Repository.Branch;
	}

	AppVeyor.UpdateBuildVersion(packageVersion);
}

//////////////////////////////////////////////////////////////////////
// DEFINE RUN CONSTANTS
//////////////////////////////////////////////////////////////////////

// Directories
var PROJECT_DIR = Context.Environment.WorkingDirectory.FullPath + "/";
var PACKAGE_DIR = PROJECT_DIR + "package/";
var TOOLS_DIR = PROJECT_DIR + "tools/";
var BIN_DIR = PROJECT_DIR + "bin/" + configuration + "/";
var TEST_NUNIT_DIR = PROJECT_DIR + "bin/nunit/";
var TEST_PACKAGES_DIR = PROJECT_DIR + "bin/packages/";
var TEST_TEAMCITY_EXT_DIR = TEST_NUNIT_DIR + "NUnit.Extension.TeamCityEventListener/tools/";

// Files
var SOLUTION_FILE = PROJECT_DIR + "teamcity-event-listener.sln";
var TEST_SOLUTION_FILE = PROJECT_DIR + "teamcity-event-listener-tests.sln";
var NUNIT3_CONSOLE = TOOLS_DIR + "NUnit.ConsoleRunner/tools/nunit3-console.exe";
var TEST_ASSEMBLY = BIN_DIR + "teamcity-event-listener.tests.dll";
var INTEGRATION_TEST_ASSEMBLY = BIN_DIR + "nunit.integration.tests.dll";

// Package sources for nuget restore
var PACKAGE_SOURCE = new string[]
	{
		"https://www.nuget.org/api/v2",
		"https://www.myget.org/F/nunit/api/v2"
	};

// Package sources for nuget restore
var PRERELEASE_PACKAGE_SOURCE = new string[]
	{
		"https://www.myget.org/F/nunit/api/v2"
	};


//////////////////////////////////////////////////////////////////////
// CLEAN
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(BIN_DIR);
});


//////////////////////////////////////////////////////////////////////
// INITIALIZE FOR BUILD
//////////////////////////////////////////////////////////////////////

Task("NuGetRestore")
    .Does(() =>
    {
		NuGetRestore(SOLUTION_FILE, new NuGetRestoreSettings()
        {
            Source = PACKAGE_SOURCE
        });
    });

//////////////////////////////////////////////////////////////////////
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Build")
    .IsDependentOn("NuGetRestore")
    .Does(() =>
    {
        DotNetBuild(SOLUTION_FILE, settings => settings
            .WithTarget("Build")
            .SetConfiguration(configuration)
            .SetVerbosity(Verbosity.Minimal)
        );
    });

//////////////////////////////////////////////////////////////////////
// TEST
//////////////////////////////////////////////////////////////////////

Task("Test")
	.IsDependentOn("Build")
	.Does(() =>
	{
		int rc = StartProcess(
			NUNIT3_CONSOLE,
			new ProcessSettings()
			{
				Arguments = TEST_ASSEMBLY
			});

		if (rc != 0)
		{
			var message = rc > 0
				? string.Format("Test failure: {0} tests failed", rc)
				: string.Format("Test exited with rc = {0}", rc);

			throw new CakeException(message);
		}
	});

//////////////////////////////////////////////////////////////////////
// INITIALIZE FOR BUILD
//////////////////////////////////////////////////////////////////////

Task("NuGetRestoreForIntegrationTests")
    .Does(() =>
    {
        NuGetRestore(TEST_SOLUTION_FILE, new NuGetRestoreSettings()
        {
            Source = PACKAGE_SOURCE
        });
    });

//////////////////////////////////////////////////////////////////////
// BUILD FOR INTEGRATION TEST
//////////////////////////////////////////////////////////////////////

Task("BuildForIntegrationTests")
    .IsDependentOn("NuGetRestoreForIntegrationTests")
    .Does(() =>
    {
        DotNetBuild(TEST_SOLUTION_FILE, settings => settings
            .WithTarget("Build")
            .SetConfiguration(configuration)
            .SetVerbosity(Verbosity.Minimal)
        );
    });

//////////////////////////////////////////////////////////////////////
// INITIALIZE FOR INTEGRATION TEST
//////////////////////////////////////////////////////////////////////

Task("InitializeForIntegrationTests")
    .Does(() =>
    {
		EnsureDirectoryExists(TEST_NUNIT_DIR);
		EnsureDirectoryExists(TEST_PACKAGES_DIR);
		CleanDirectories(TEST_NUNIT_DIR + "**/*.*");
		CleanDirectories(TEST_PACKAGES_DIR + "**/*.*");		

		NuGetInstall(new [] {"NUnit.Runners", "NUnit"}, new NuGetInstallSettings()
        {
			OutputDirectory = TEST_NUNIT_DIR,
            Source = PRERELEASE_PACKAGE_SOURCE,
			Prerelease = true,
			NoCache = true
        });

		NuGetInstall(new [] {"NUnit"}, new NuGetInstallSettings()
        {
			Version = "2.6.4",
			OutputDirectory = TEST_PACKAGES_DIR,
            Source = PACKAGE_SOURCE,
			Prerelease = true,
			NoCache = true
        });		

		CleanDirectories(TEST_NUNIT_DIR + "NUnit.Extension.TeamCityEventListener*/**/*.*");
		EnsureDirectoryExists(TEST_TEAMCITY_EXT_DIR);				
		CopyFileToDirectory(BIN_DIR + "teamcity-event-listener.dll", TEST_TEAMCITY_EXT_DIR);
    });

//////////////////////////////////////////////////////////////////////
// INTEGRATION TEST
//////////////////////////////////////////////////////////////////////

Task("IntegrationTest")
	.IsDependentOn("Build")
	.IsDependentOn("BuildForIntegrationTests")
	.IsDependentOn("InitializeForIntegrationTests")
	.Does(() =>
	{
		int rc = StartProcess(
			NUNIT3_CONSOLE,
			new ProcessSettings()
			{
				Arguments = INTEGRATION_TEST_ASSEMBLY
			});

		if (rc != 0)
		{
			var message = rc > 0
				? string.Format("Test failure: {0} tests failed", rc)
				: string.Format("Test exited with rc = {0}", rc);

			throw new CakeException(message);
		}
	});


//////////////////////////////////////////////////////////////////////
// PACKAGE
//////////////////////////////////////////////////////////////////////

Task("Package")
    .IsDependentOn("Build")
    .Does(() => 
    {
        CreateDirectory(PACKAGE_DIR);

        NuGetPack("teamcity-event-listener.nuspec", new NuGetPackSettings()
        {
            Version = packageVersion,
            BasePath = BIN_DIR,
            OutputDirectory = PACKAGE_DIR
        });
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Rebuild")
    .IsDependentOn("Clean")
	.IsDependentOn("Build");

Task("Appveyor")
	.IsDependentOn("Build")
	.IsDependentOn("Test")
	.IsDependentOn("IntegrationTest")
	.IsDependentOn("Package");

Task("Travis")
	.IsDependentOn("Build")
	.IsDependentOn("Test");

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
