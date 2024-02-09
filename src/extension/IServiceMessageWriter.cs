namespace NUnit.Engine.Listeners
{
    using System.Collections.Generic;
    using System.IO;

    public interface IServiceMessageWriter
    {
        void Write(TextWriter writer, IEnumerable<ServiceMessage> serviceMessages);
        void Write(TextWriter writer, ServiceMessage serviceMessages);
    }
}