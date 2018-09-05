namespace NUnit.Engine.Listeners
{
    using System.Collections.Generic;
    using System.IO;

    public interface IServiceMessageWriter
    {
        void Write(TextWriter writer, IEnumerable<ServiceMessage> serviceMessages);
    }
}