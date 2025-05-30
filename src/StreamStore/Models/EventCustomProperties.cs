using System.Collections.Generic;
using System.Linq;
using StreamStore.Extensions;



namespace StreamStore
{
    internal class EventCustomProperties : Dictionary<string, string>
    {
        public static EventCustomProperties Empty() => new EventCustomProperties();

        public EventCustomProperties(): base()
        {
        }

        public EventCustomProperties(IEnumerable<KeyValuePair<string, string>>? properties) : 
            base(properties.NotNullAndNotEmpty() ? properties: Enumerable.Empty<KeyValuePair<string, string>>())
        {
        }
    }
}
