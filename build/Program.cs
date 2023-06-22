// Run this from the working directory where the solution or project to build is located.

using System.CommandLine;
using Build;using HostApi;
using NuGet.Versioning;
using Pure.DI;

Directory.SetCurrentDirectory(Tools.GetSolutionDirectory());
Directory.CreateDirectory(".bin");
var settings = new Settings(
    "Release",
    VersionRange.Parse(Property.Get("version", "1.*", true)),
    Property.Get("NuGetKey", string.Empty));

var composition = new Composition(settings);
return await composition.Root.RunAsync();

#pragma warning disable CS0162
// ReSharper disable once HeuristicUnreachableCode
DI.Setup("Composition")
    .Arg<Settings>("settings")
    .Bind<INuGet>().To(_ => Host.GetService<INuGet>())
    .Bind<ITarget<string>>("pack").To<PackTarget>()
    .Bind<ITarget<ToolSettings>>().To<RestoreToolsTarget>()
    .Bind<ToolSettings>().As(Lifetime.Singleton).To(ctx =>
    {
        ctx.Inject<ITarget<ToolSettings>>(out var restoreToolsTarget);
        return restoreToolsTarget.RunAsync().Result;
    })
    .Bind<ITarget<int>>("deploy").To<DeployTarget>()
    .Root<Program>("Root");
    
internal partial class Program
{
    private readonly RootCommand _rootCommand;

    public Program(
        [Tag("pack")] ITarget<string> pack,
        ITarget<ToolSettings> restoreTools,
        [Tag("deploy")] ITarget<int> deploy)
    {
        var packCommand = new Command("pack", "Creates NuGet packages");
        packCommand.SetHandler(pack.RunAsync);
        packCommand.AddAlias("p");
        
        var restoreToolsCommand = new Command("restore", "Restores tools");
        restoreToolsCommand.SetHandler(restoreTools.RunAsync);
        restoreToolsCommand.AddAlias("r");

        var deployCommand = new Command("deploy", "Push NuGet packages");
        deployCommand.SetHandler(deploy.RunAsync);
        deployCommand.AddAlias("d");
        
        _rootCommand = new RootCommand
        {
            packCommand,
            deployCommand
        };
    }

    private Task<int> RunAsync() => _rootCommand.InvokeAsync(Args.ToArray());
}
