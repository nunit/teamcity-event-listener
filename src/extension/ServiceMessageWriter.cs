// ***********************************************************************
// Copyright (c) 2015 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

namespace NUnit.Engine.Listeners
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Collections.Generic;
    using System.Text;

    [SuppressMessage("ReSharper", "UseNameofExpression")]
    public class ServiceMessageWriter : IServiceMessageWriter
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

        public static string EscapeString(string value)
        {
            var sb = new StringBuilder(value.Length * 2);
            foreach (var ch in value)
            {
                switch (ch)
                {
                    case '|':
                        sb.Append("||");
                        break; //
                    case '\'':
                        sb.Append("|'");
                        break; //
                    case '\n':
                        sb.Append("|n");
                        break; //
                    case '\r':
                        sb.Append("|r");
                        break; //
                    case '[':
                        sb.Append("|[");
                        break; //
                    case ']':
                        sb.Append("|]");
                        break; //
                    case '\u0085':
                        sb.Append("|x");
                        break; //\u0085 (next line)=>|x
                    case '\u2028':
                        sb.Append("|l");
                        break; //\u2028 (line separator)=>|l
                    case '\u2029':
                        sb.Append("|p");
                        break; //
                    default:
                        if (ch > 127)
                        {
                            sb.Append(string.Format("|0x{0:x4}", (ulong) ch));
                        }
                        else
                        {
                            sb.Append(ch);
                        }

                        break;
                }
            }

            return sb.ToString();
        }
    }
}
