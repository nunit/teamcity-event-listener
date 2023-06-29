namespace nunit.integration.tests.Dsl
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    internal class TestSession
    {
        private readonly TestContext _context;
        
        public TestSession(TestContext context, int exitCode, string output, IEnumerable<Process> processesAfter)
        {
            _context = context;
            ExitCode = exitCode;
            Output = output;
            ProcessesAfter = processesAfter.ToList();
        }

        public int ExitCode { get; }

        public string Output { get; }

        public IList<Process> ProcessesAfter { get; }

        public override string ToString()
        {
            return $"cd {_context.SandboxPath}";
        }
    }
}
