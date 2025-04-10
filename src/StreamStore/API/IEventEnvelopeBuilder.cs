using System;


namespace StreamStore
{
    public interface IEventEnvelopeBuilder
    {
        IEventEnvelopeBuilder WithId(Id id);
        IEventEnvelopeBuilder Dated(DateTime timestamp);
        IEventEnvelopeBuilder WithEvent(object @event);
    }
}
