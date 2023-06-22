namespace NUnit.Engine.Listeners
{
    using Pure.DI;

    internal partial class Composition
    {
        private static void Setup() => DI
            .Setup(nameof(Composition))
            .Bind<IHierarchy>().To<Hierarchy>().Root<IHierarchy>();
    }
}