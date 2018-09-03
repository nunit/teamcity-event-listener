using System.Text;

namespace nunit.integration.tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Dsl;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class CommonSteps
    {
        [Given(@"I have changed current directory to (.+)")]
        public void ChangeCurrentDirectory(string newCurrentDirectory)
        {
            var ctx = ScenarioContext.Current.GetTestContext();
            newCurrentDirectory = Path.GetFullPath(Path.Combine(ctx.SandboxPath, newCurrentDirectory));
            if (!Directory.Exists(newCurrentDirectory))
            {
                Directory.CreateDirectory(newCurrentDirectory);
            }

            ctx.CurrentDirectory = newCurrentDirectory;
        }

        [Given(@"I have specified encoding (.+)")]
        public void SpecifyEncoding(string encoding)
        {
            var ctx = ScenarioContext.Current.GetTestContext();
            ctx.Encoding = Encoding.GetEncoding(encoding);
        }

        [Given(@"I have appended the string (.+) to file (.+)")]
        public void AppendStringToFile(string content, string fileName)
        {
            var ctx = ScenarioContext.Current.GetTestContext();
            File.AppendAllText(Path.GetFullPath(Path.Combine(ctx.SandboxPath, fileName)), content);            
        }

        [Given(@"I have appended the line (.+) to file (.+)")]
        public void AppendLineFile(string line, string fileName)
        {
            AppendStringToFile(line + Environment.NewLine, fileName);
        }

        [Then(@"the xml file (.+) contains items by xPath (.+):")]
        public void XmlFileShouldContainAttributes(string xmlFileName, string xPathExpression, Table data)
        {
            var ctx = ScenarioContext.Current.GetTestContext();
            var items = new XmlParser().Parse(Path.GetFullPath(Path.Combine(ctx.SandboxPath, xmlFileName)), xPathExpression).ToList();
                        
            Assert.AreEqual(data.RowCount, items.Count, $"{ctx}\nExpected count of items is {data.RowCount} but actual is {items.Count} in the file \"{xmlFileName}\"");            

            var invalidItems = (
                from item in data.Rows.Zip(items, (row, item) => new  { row, item })
                where !VerifyItem(item.row, item.item)
                select item).ToList();

            if (invalidItems.Any())
            {
                var details = string.Join("\n", invalidItems.Select(i => CreateErrorMessage(i.row, i.item)));
                Assert.Fail($"See {ctx}\n{details}");
            }
        }

        [Given(@"I have added the environment variable (.+) as (.+)")]
        public void AddEnvVar(string name, string value)
        {
            var ctx = ScenarioContext.Current.GetTestContext();
            var configuration = ctx.GetOrCreateNUnitConfiguration();
            configuration.AddRawEnvVariable(new RawEnvVariable(name, value));
        }

        private bool VerifyItem(TableRow row, IEnumerable<ItemValue> item)
        {
            var vals = item.ToDictionary(i => i.Name, i => i.Value);
            foreach (var key in row.Keys)
            {
                if (key == null)
                {
                    continue;
                }

                string rowValue;
                if (!row.TryGetValue(key, out rowValue))
                {
                    continue;
                }

                string val;
                if (!vals.TryGetValue(key, out val))
                {
                    return false;
                }

                if (!StringComparer.InvariantCultureIgnoreCase.Equals(rowValue, val))
                {
                    return false;
                }
            }

            return true;
        }

        private static string CreateErrorMessage(TableRow row, IEnumerable<ItemValue> item)
        {
            var rowInfo = string.Join(", ", from key in row.Keys select $"{key} = {row[key]}");
            var itemInfo = string.Join(", ", from val in item select $"{val.Name} = {val.Value}");

            return $"Expected item should has:\n{rowInfo}\nbut it has:\n{itemInfo}";
        }
    }
}
