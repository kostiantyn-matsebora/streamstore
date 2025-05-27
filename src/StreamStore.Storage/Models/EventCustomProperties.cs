using System.Collections.Generic;
using StreamStore.Models;


namespace StreamStore.Storage.Models
{
    public class EventCustomProperties : Dictionary<string, string>, ICustomProperties
    {
        public static EventCustomProperties Empty() => new EventCustomProperties();

        public EventCustomProperties(): base()
        {
        }

        public EventCustomProperties(IDictionary<string, string> properties) : base(properties)
        {
        }
    }
}
