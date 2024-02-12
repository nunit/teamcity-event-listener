namespace NUnit.Engine.Listeners
{
    using System.IO;

    public interface IServiceMessageWriter
    {
        void Write(TextWriter writer, ServiceMessage serviceMessages);
    }
}