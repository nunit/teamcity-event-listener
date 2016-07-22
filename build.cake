#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.1

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// SET PACKAGE VERSION
//////////////////////////////////////////////////////////////////////

var version = "3.5.0";
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

var packageName = "NUnit3TestAdapter-" + packageVersion;

//////////////////////////////////////////////////////////////////////
// DEFINE RUN CONSTANTS
//////////////////////////////////////////////////////////////////////

// Directories
var PROJECT_DIR = Context.Environment.WorkingDirectory.FullPath + "/";
var PACKAGE_DIR = PROJECT_DIR + "package/";
var PACKAGE_IMAGE_DIR = PACKAGE_DIR + packageName + "/";
var TOOLS_DIR = PROJECT_DIR + "tools/";
var BIN_DIR = PROJECT_DIR + "bin/" + configuration + "/";

// Solutions
var SOLUTION_FILE = PROJECT_DIR + "teamcity-event-listener.sln";

// Test Runner
var NUNIT3_CONSOLE = TOOLS_DIR + "NUnit.ConsoleRunner/tools/nunit3-console.exe";

// Test Assemblies
var TEST_ASSEMBLY = BIN_DIR + "NUnit.VisualStudio.TestAdapter.Tests.dll";

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
    NuGetRestore(SOLUTION_FILE);
});

//////////////////////////////////////////////////////////////////////
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Build")
    .IsDependentOn("NuGetRestore")
    .Does(() =>
    {
		BuildSolution(SOLUTION_FILE, configuration);
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
// PACKAGE
//////////////////////////////////////////////////////////////////////

Task("CreatePackageDir")
	.Does(() =>
	{
		CreateDirectory(PACKAGE_DIR);
	});

Task("CreateWorkingImage")
	.IsDependentOn("Build")
	.IsDependentOn("CreatePackageDir")
	.Does(() =>
	{
		CreateDirectory(PACKAGE_IMAGE_DIR);
		CleanDirectory(PACKAGE_IMAGE_DIR);

		CopyFileToDirectory("LICENSE.txt", PACKAGE_IMAGE_DIR);

		var binFiles = new FilePath[]
		{
			BIN_DIR + "teamcity-event-listener.dll",
			BIN_DIR + "nunit.engine.api.dll"
		};

		var binDir = PACKAGE_IMAGE_DIR + "bin/";
		CreateDirectory(binDir);
		CopyFiles(binFiles, binDir);
	});

Task("Package")
	.IsDependentOn("CreateWorkingImage")
	.Does(() => 
	{
        NuGetPack("teamcity-event-listener.nuspec", new NuGetPackSettings()
        {
            Version = packageVersion,
            BasePath = BIN_DIR,
            OutputDirectory = PACKAGE_DIR
        });
	});

//////////////////////////////////////////////////////////////////////
// HELPER METHODS
//////////////////////////////////////////////////////////////////////

void BuildSolution(string solutionPath, string configuration)
{
	MSBuild(solutionPath, new MSBuildSettings()
		.SetConfiguration(configuration)
        .SetMSBuildPlatform(MSBuildPlatform.x86)
		.SetVerbosity(Verbosity.Minimal)
		.SetNodeReuse(false)
	);
}

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Rebuild")
    .IsDependentOn("Clean")
	.IsDependentOn("Build");

Task("Appveyor")
	.IsDependentOn("Build")
	.IsDependentOn("Test")
	.IsDependentOn("Package");

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
