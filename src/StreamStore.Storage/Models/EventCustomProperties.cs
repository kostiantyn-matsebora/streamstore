using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StreamStore.Extensions;
using StreamStore.Models;


namespace StreamStore.Storage.Models
{
    public class EventCustomProperties : Dictionary<string, string>, ICustomProperties
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
