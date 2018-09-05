namespace NUnit.Engine.Listeners
{
    using System.Collections.Generic;

    public interface IEventConverter
    {
        IEnumerable<IEnumerable<ServiceMessage>> Convert(Event testEvent);
    }
}
