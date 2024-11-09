using System;


namespace StreamStore
{
    public interface IEventBuilder
    {
        IEventBuilder WithId(Id id);
        IEventBuilder Dated(DateTime timestamp);
        IEventBuilder WithEvent(object @event);
    }
}
