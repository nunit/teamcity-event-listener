// ReSharper disable StringLiteralTypo
// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable HeapView.ClosureAllocation
// ReSharper disable ClassNeverInstantiated.Global
namespace Build;

using HostApi;

internal class PackTarget: ITarget<string>
{
    private readonly Settings _settings;
    private readonly ToolSettings _toolSettings;

    public PackTarget(
        Settings settings,
        ToolSettings toolSettings)
    {
        _settings = settings;
        _toolSettings = toolSettings;
    }

    public async Task<string> RunAsync()
    {
        var packageVersion = Tools.GetNextVersion(new NuGetRestoreSettings("NUnit.Extension.TeamCityEventListener"), _settings.VersionRange);
        var props = new[]
        {
            ("configuration", "Release"),
            ("version", packageVersion.ToString())
        };
        
        var build = new MSBuild()
            .WithProject("teamcity-event-listener.sln")
            .WithTarget("rebuild;pack")
            .WithProps(props)
            .WithRestore(true);

        var buildResult = await build.BuildAsync();
        Assertion.Succeed(buildResult);

        var project = Path.Combine("src", "tests", "teamcity-event-listener.tests.csproj");
        var baseOutput = Path.Combine(".bin");
        foreach (var framework in new [] {"net20", "netstandard2.0"})
        {
            var output = Path.Combine(baseOutput, framework);
            var result = await new DotNetPublish()
                .WithProject(project)
                .WithNoLogo(true)
                .WithNoBuild(true)
                .WithFramework(framework)
                .WithOutput(Path.Combine(output, framework))
                .RunAsync();
            
            Assertion.Succeed(result, "Publish");
            
            result = await new CommandLine(_toolSettings.NUnitExecutable)
                .WithWorkingDirectory(output)
                .WithArgs("teamcity-event-listener.tests.dll")
                .RunAsync();
            
            Assertion.Succeed(result, "Tests");
        }

        return "";
    }
}