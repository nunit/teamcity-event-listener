namespace nunit.integration.tests.Dsl
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal class TestOutputSummaryParser : IParser<OutputSummary>
    {
        private static readonly string[][] FieldsSets = {
            new[] { "Test Count", "Passed", "Failed", "Inconclusive", "Skipped" },
            new[] { "Test Count", "Passed", "Failed", "Warnings", "Inconclusive", "Skipped" }
        };

        public OutputSummary Parse(string text)
        {
            var matchResult = (
                from fieldsSet in FieldsSets
                let summaryRegex = CreateSummaryRegex(fieldsSet)
                let match = summaryRegex.Match(text)
                where match.Success
                select new {fieldsSet, match}
            ).FirstOrDefault();

            if (matchResult == null)
            {
                return new OutputSummary(new Dictionary<string, string>());
            }

            var vals = (
                from field in matchResult.fieldsSet
                let valMatch = matchResult.match.Groups[GetKey(field)]
                where valMatch.Success
                select new {field, value = valMatch.Value}).ToDictionary(i => i.field, i => i.value);

            return new OutputSummary(vals);
        }

        private static Regex CreateSummaryRegex(IEnumerable<string> fields)
        {
            return new Regex(string.Join(@"(,|)\s*", fields.Select(f => $"{f}:\\s*(?<{GetKey(f)}>\\d+)")), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        }

        private static string GetKey(string field)
        {
            return field.Replace(" ", string.Empty);
        }
    }
}
