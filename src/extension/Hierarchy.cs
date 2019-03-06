namespace NUnit.Engine.Listeners
{
    using System;
    using System.Collections.Generic;

    internal class Hierarchy : IHierarchy
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
