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
    using System.Collections.Generic;

    public class Hierarchy : IHierarchy
    {
        private readonly Dictionary<string, string> _links = new Dictionary<string, string>();

        public bool AddLink(string childId, string parentId)
        {           
            string curParentId;
            if (!_links.TryGetValue(childId, out curParentId) || curParentId != parentId)
            {
                _links[childId] = parentId;
                return true;
            }

            return false;
        }
        
        public void Clear()
        {
            _links.Clear();
        }

        public bool TryFindRootId(string childId, out string rootId)
        {
            if (childId == null)
            {
                throw new ArgumentNullException("childId");
            }

            while (TryFindParentId(childId, out rootId) && childId != rootId)
            {
                childId = rootId;
            }

            rootId = childId;
            return !string.IsNullOrEmpty(childId);
        }

        public bool TryFindParentId(string childId, out string parentId)
        {
            if (childId == null)
            {
                throw new ArgumentNullException("childId");
            }

            var result = _links.TryGetValue(childId, out parentId) && !string.IsNullOrEmpty(parentId);
            if (result && childId == parentId)
            {
                return false;
            }

            return result;
        }
    }
}
