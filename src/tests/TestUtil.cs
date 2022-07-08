namespace NUnit.Engine.Listeners
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;

    // ReSharper disable once InconsistentNaming
    public static class TestUtil
    {
        private const string StartBlock = "!!!!{ ";
        private const string FinishBlock = " }!!!!";

        public static EventListener CreateListener(TextWriter outWriter, ITeamCityInfo info = null)
        {
            var statistics = new Statistics();
            if (info == null)
            {
                info = new TeamCityInfo();
            }

            var serviceMessageFactory = new ServiceMessageFactory(info, new SuiteNameReplacer(info));
            return new EventListener(
                outWriter,
                new TeamCityInfo(),
                new Statistics(),
                new ServiceMessageWriter(),
                new EventConverter2(serviceMessageFactory, new Hierarchy(), statistics, info),
                new EventConverter3(serviceMessageFactory, new Hierarchy(), statistics, info))
            {
                RootFlowId = string.Empty
            };
        }

        public static XmlNode CreateStartRun(int count)
        {
            return CreateMessage(string.Format("<start-run count='{0}'/>", count));
        }

        public static XmlNode CreateTestRun()
        {
            return CreateMessage("<test-run></test-run>");
        }

        public static XmlNode CreateStartSuite(string id, string parentId, string name)
        {
            return CreateMessage(string.Format("<start-suite id=\"{0}\" {1} name=\"{2}\" fullname=\"{2}_abc\"/>", id, GetNamedAttr("parentId", parentId), name));
        }

        public static XmlNode CreateFinishSuite(string id, string parentId, string name, string type)
        {
            return CreateMessage(string.Format("<test-suite id=\"{0}\" {1} type=\"{3}\" name=\"{2}\" fullname=\"{2}_abc\" runstate=\"Runnable\" testcasecount=\"3\" result=\"Failed\" duration=\"0.251125\" total=\"3\" passed=\"0\" failed=\"0\" inconclusive=\"0\" skipped=\"0\" asserts=\"0\"><failure><message><![CDATA[One or more child tests had errors]]></message><stack-trace><![CDATA[stack abc]]></stack-trace></failure></test-suite>", id, GetNamedAttr("parentId", parentId), name, type));
        }

        public static XmlNode CreateStartTest(string id, string parentId, string name)
        {
            return CreateMessage(string.Format("<start-test id=\"{0}\" {1} name=\"{2}\" fullname=\"{2}\"/>", id, GetNamedAttr("parentId", parentId), name));
        }

        public static XmlNode CreateTestCaseSuccessful(string id, string parentId, string name, string duration, string output)
        {
            return CreateMessage(string.Format("<test-case id=\"{0}\" {1} name=\"{2}\" fullname=\"{2}\" runstate=\"Runnable\" result=\"Passed\" duration=\"{3}\" asserts=\"0\">{4}</test-case>", id, GetNamedAttr("parentId", parentId), name, duration, GetNamedElement("output", output)));
        }

        public static XmlNode CreateTestCaseFailed(string id, string parentId, string name, string duration, string message, string stackTrace)
        {
            return CreateMessage(string.Format("<test-case id=\"{0}\" {1} name=\"{2}\" fullname=\"{2}\" runstate=\"Runnable\" result=\"Failed\" duration=\"{3}\" asserts=\"0\"><properties><property name=\"_CATEGORIES\" value=\"F\" /></properties><failure><message><![CDATA[{4}]]></message><stack-trace><![CDATA[{5}]]></stack-trace></failure></test-case>", id, GetNamedAttr("parentId", parentId), name, duration, message, stackTrace));
        }
        
        public static IEnumerable<XmlNode> ConvertToMessages(IEnumerable<string> lines)
        {
            var fullLine = new StringBuilder();
            var inBlock = false;
            foreach (var line in lines)
            {
                if (!inBlock)
                {
                    if (line.Contains(StartBlock))
                    {
                        inBlock = true;
                        fullLine.Length = 0;
                    }
                }

                if (!inBlock)
                {
                    continue;
                }

                fullLine.AppendLine(line);
                if (!line.Contains(FinishBlock))
                {
                    continue;
                }

                inBlock = false;
                var nextLine = fullLine.ToString();
                var first = nextLine.IndexOf(StartBlock, StringComparison.InvariantCulture) + StartBlock.Length;
                var last = nextLine.IndexOf(FinishBlock, StringComparison.InvariantCulture);
                yield return CreateMessage(nextLine.Substring(first, last - first));
            }
        }

        private static string GetNamedAttr(string attrName, string attrValue)
        {
            return attrValue == null 
                ? string.Empty
                : string.Format("{0}=\"{1}\"", attrName, attrValue);
        }

        private static string GetNamedElement(string elementName, string elementValue)
        {
            return elementValue == null
                ? string.Empty
                : string.Format("<{0}>{1}</{0}>", elementName, elementValue);
        }

        private static XmlNode CreateMessage(string text)
        {
            var doc = new XmlDocument();
            doc.LoadXml(text);
            return doc.FirstChild;
        }
    }
}