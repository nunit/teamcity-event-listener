namespace NUnit.Engine.Listeners
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Collections.Generic;

    [SuppressMessage("ReSharper", "UseNameofExpression")]
    internal class ServiceMessageWriter : IServiceMessageWriter
    {
        private const string Header = "##teamcity[";
        private const string Footer = "]";

        public void Write(TextWriter writer, IEnumerable<ServiceMessage> serviceMessages)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            foreach (var serviceMessage in serviceMessages)
            {
                writer.Write(Header);
                writer.Write(serviceMessage.Name);

                if (!string.IsNullOrEmpty(serviceMessage.Value))
                {
                    writer.Write(' ');
                    Write(writer, serviceMessage.Value);
                }
                else
                {
                    foreach (var attribute in serviceMessage.Attributes)
                    {
                        writer.Write(' ');
                        Write(writer, attribute);
                    }
                }

                writer.WriteLine(Footer);
            }
        }

        private static void Write(TextWriter writer, ServiceMessageAttr attribute)
        {
            writer.Write(attribute.Name);
            writer.Write("='");
            writer.Write(EscapeString(attribute.Value));
            writer.Write('\'');
        }

        private static void Write(TextWriter writer, string value)
        {
            writer.Write('\'');
            writer.Write(EscapeString(value));
            writer.Write('\'');
        }

        private static string EscapeString(string value)
        {
            return value != null
                ? value.Replace("|", "||")
                       .Replace("'", "|'")
                       .Replace("’", "|’")
                       .Replace("\n", "|n")
                       .Replace("\r", "|r")
                       .Replace(char.ConvertFromUtf32(int.Parse("0086", NumberStyles.HexNumber)), "|x")
                       .Replace(char.ConvertFromUtf32(int.Parse("2028", NumberStyles.HexNumber)), "|l")
                       .Replace(char.ConvertFromUtf32(int.Parse("2029", NumberStyles.HexNumber)), "|p")
                       .Replace("[", "|[")
                       .Replace("]", "|]")
                : null;
        }
    }
}
