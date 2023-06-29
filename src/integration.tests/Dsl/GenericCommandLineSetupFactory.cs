namespace nunit.integration.tests.Dsl
{
    using System.Linq;

    internal class GenericCommandLineSetupFactory : ICommandLineSetupFactory
    {
        private readonly string _executable;
        private const string ConfigFileResourceName = "nunit.integration.tests.Templates.App.config";
        private readonly static ResourceManager ResourceManager = new ResourceManager();

        public GenericCommandLineSetupFactory(string executable)
        {
            _executable = executable;
        }

        public CommandLineSetup Create(TestContext ctx)
        {
            var configuration = ctx.GetOrCreateNUnitConfiguration();
            var configFiles =
                from configFile in configuration.ConfigFiles
                select new CommandLineArtifact(configFile.ConfigFileName, ResourceManager.GetContentFromResource(ConfigFileResourceName));
            var artifacts = configFiles.ToArray();

            return new CommandLineSetup(
                _executable,
                ctx.CurrentDirectory,                
                string.Join(" ", configuration.AssemblyFileNames)
                + " "
                + string.Join(" ", configuration.Args.Select(arg => arg.ConvertToString(configuration.NUnitVersion))),
                configuration.EnvVariables.ToDictionary(envVariable => envVariable.GetName(configuration.NUnitVersion), envVariable => envVariable.GetValue(configuration.NUnitVersion)),
                artifacts);
        }
    }
}
