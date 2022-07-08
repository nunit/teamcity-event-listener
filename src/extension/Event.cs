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
    using System.Xml;

    public struct Event
    {
        public readonly string RootFlowId;
        public readonly string MessageName;
        public readonly string FullName;
        public readonly string Name;
        public readonly string Id;
        public readonly string ParentId;
        public readonly string TestId;
        // ReSharper disable once InconsistentNaming
        public readonly XmlNode TestEvent;

        public Event(
            string rootFlowId,
            string messageName,
            string fullName,
            string name,
            string id,
            string parentId,
            string testId,
            XmlNode testEvent)
        {
            RootFlowId = rootFlowId;
            MessageName = messageName;
            FullName = fullName;
            Name = name;
            Id = id;
            ParentId = parentId;
            TestId = testId;
            TestEvent = testEvent;
        }

        public override string ToString()
        {
            return string.Format("RootFlowId: {0}, MessageName: {1}, FullName: {2}, Id: {3}, ParentId: {4}, Event: {5}", RootFlowId, MessageName, FullName, Id, ParentId, TestEvent.OuterXml);
        }
    }
}
