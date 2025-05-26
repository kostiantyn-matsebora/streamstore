using System;
using System.Collections.Generic;


namespace StreamStore.Models
{
    public interface ICustomProperties : IReadOnlyDictionary<string, string>
    {
        string GetPropertyValue(string key);
        T GetPropertyValue<T>(string key, Func<string, T> converter);
    }
}
