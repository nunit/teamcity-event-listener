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
    using System.Diagnostics.CodeAnalysis;
    using Extensibility;

    // Note: Setting minimum engine version in this case is
    // purely documentary since engines prior to 3.4 do not
    // check the EngineVersion property and will try to
    // load this extension anyway.
    [Extension(Enabled = false, EngineVersion = "3.4")]
    [SuppressMessage("ReSharper", "UseNameofExpression")]
    public class TeamCityEventListener : ITestEventListener
    {
        private readonly ITestEventListener _listener;

        // ReSharper disable once UnusedMember.Global
        public TeamCityEventListener()
        {
            _listener = Composer.ResolveITestEventListener();
        }

        public void OnTestEvent(string report)
        {
            _listener.OnTestEvent(report);
        }
    }
}
