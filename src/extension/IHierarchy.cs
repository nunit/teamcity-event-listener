namespace NUnit.Engine.Listeners
{
    internal interface IHierarchy
    {
        bool AddLink(string childId, string parentId);

        void Clear();

        bool TryFindRootId(string childId, out string rootId);

        bool TryFindParentId(string childId, out string parentId);
    }
}