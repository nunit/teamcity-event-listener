namespace nunit.integration.tests.Dsl
{
    internal class RawEnvVariable
    {
        public RawEnvVariable(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public string Value { get; }
    }
}
