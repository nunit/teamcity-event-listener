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

// ReSharper disable UnusedMember.Local
namespace NUnit.Engine.Listeners
{
    using System;
    using System.IO;
    using Pure.DI;

    public static partial class Composer
    {
        private static void Setup()
        {
            DI.Setup()
                .Bind<TextWriter>().To(ctx => Console.Out)
                .Bind<Statistics>().To<Statistics>()
                .Bind<IServiceMessageWriter>().To<ServiceMessageWriter>()
                .Bind<ITeamCityInfo>().To<TeamCityInfo>()
                .Bind<ISuiteNameReplacer>().To<SuiteNameReplacer>()
                .Bind<IServiceMessageFactory>().To<ServiceMessageFactory>()
                .Bind<IHierarchy>().To<Hierarchy>()
                .Bind<IEventConverter>(2).To<EventConverter2>()
                .Bind<IEventConverter>(3).To<EventConverter3>()
                .Bind<ITestEventListener>().To<EventListener>();
        }
    }
}