using System;
using System.Collections.Generic;
using StreamStore.Exceptions;
using StreamStore.Models;


namespace StreamStore.Storage.Models
{
    public class EventCustomProperties : Dictionary<string, string>, ICustomProperties
    {
        public static EventCustomProperties Empty { get; } = new EventCustomProperties();

        public string GetPropertyValue(string key)
        {
            return GetPropertyValue(key, s => s);
        }

        public T GetPropertyValue<T>(string key, Func<string, T> converter)
        {
            if (!TryGetValue(key, out var value))
            {
                throw new CustomPropertyKeyNotFoundException(key);
            }

            try
            {
                return converter(value);
            }
            catch (Exception e)
            {
                throw new CustomPropertyParseException(key, value, e);
            }
        }
    }
}
