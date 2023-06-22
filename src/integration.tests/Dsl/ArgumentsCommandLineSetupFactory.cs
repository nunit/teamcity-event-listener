namespace nunit.integration.tests.Dsl
{
    using System;
    using System.IO;
    using System.Linq;

    internal class ArgumentsCommandLineSetupFactory : ICommandLineSetupFactory
    {
        private const string ConfigFileResourceName = "nunit.integration.tests.Templates.App.config";
        private readonly static ResourceManager ResourceManager = new ResourceManager();

        public CommandLineSetup Create(TestContext ctx)
        {
            var configuration = ctx.GetOrCreateNUnitConfiguration();
            var configFiles =
                from configFile in configuration.ConfigFiles
                select new CommandLineArtifact(configFile.ConfigFileName, ResourceManager.GetContentFromResource(ConfigFileResourceName));
            var artifacts = configFiles.ToArray();

            var envVars = configuration.EnvVariables.Select(i => Tuple.Create(i.GetName(configuration.NUnitVersion), i.GetValue(configuration.NUnitVersion)));
            var rawEnvVars = configuration.RawEnvVariables.Select(i => Tuple.Create(i.Name, i.Value));
            return new CommandLineSetup(
                Path.Combine(configuration.NUnitConsolePath, Const.NUnitConsoleFileName),
                ctx.CurrentDirectory,
                string.Join(" ", configuration.AssemblyFileNames)
                + " "
                + string.Join(" ", configuration.Args.Select(arg => arg.ConvertToString(configuration.NUnitVersion))),
                envVars.Concat(rawEnvVars).ToDictionary(i => i.Item1, i => i.Item2),
                artifacts);
        }
    }
}
