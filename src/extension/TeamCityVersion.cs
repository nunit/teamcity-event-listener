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
    using System.Text.RegularExpressions;

    public class TeamCityVersion: IComparable<TeamCityVersion>
    {
        private static readonly Regex VersionRegex = new Regex("^(\\d+)(\\.(\\d+)|).*", RegexOptions.Compiled);

        public TeamCityVersion(string version)
        {
            try
            {
                if (!string.IsNullOrEmpty(version))
                {
                    var match = VersionRegex.Match(version);
                    if (match.Success)
                    {
                        int val;
                        if (int.TryParse(match.Groups[1].Value, out val))
                        {
                            Major = val;
                        }

                        if (int.TryParse(match.Groups[3].Value, out val))
                        {
                            Minor = val;
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        public int Major { get; private set; }

        public int Minor { get; private set; }

        public int CompareTo(TeamCityVersion other)
        {
            var result = Major.CompareTo(other.Major);
            if (result != 0)
            {
                return result;
            }

            return Minor.CompareTo(other.Minor);
        }

        public override string ToString()
        {
            return Major + "." + Minor;
        }
    }
}
