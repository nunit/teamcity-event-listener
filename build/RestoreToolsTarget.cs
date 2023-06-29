// ReSharper disable StringLiteralTypo
// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable HeapView.ClosureAllocation
// ReSharper disable ClassNeverInstantiated.Global
namespace Build;

using HostApi;

internal class RestoreToolsTarget: ITarget<ToolSettings>
{
    private readonly INuGet _nuGet;

    public RestoreToolsTarget(INuGet nuGet) =>
        _nuGet = nuGet;

    public Task<ToolSettings> RunAsync()
    {
        var nunitPackages = Path.Combine(".bin", "nunit");
        if (Directory.Exists(nunitPackages))
        {
            Directory.Delete(nunitPackages, true);
        }

        Directory.CreateDirectory(nunitPackages);
        _nuGet.Restore(new NuGetRestoreSettings("NUnit.Console").WithPackagesPath(nunitPackages));
        var nunitExecutable = Directory.EnumerateFiles(nunitPackages, "nunit3-console.exe", SearchOption.AllDirectories).Single();
        return Task.FromResult(new ToolSettings(nunitExecutable));
    }
}