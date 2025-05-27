using System;
using System.Collections.Generic;


namespace StreamStore
{
    public interface IEventEnvelopeBuilder
    {
        IEventEnvelopeBuilder WithId(Id id);
        IEventEnvelopeBuilder Dated(DateTime timestamp);
        IEventEnvelopeBuilder WithEvent(object @event);
        IEventEnvelopeBuilder WithCustomProperties(IEnumerable<KeyValuePair<string, string>> keyValuePairs);
        IEventEnvelopeBuilder WithCustomProperty(string key, string value);
    }
}
