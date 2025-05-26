using System;


namespace StreamStore.Exceptions
{
    public sealed class CustomPropertyKeyNotFoundException : CustomPropertyException
    {
        public CustomPropertyKeyNotFoundException(string key)
          : base($"Could not find custom property key '{key}'")
        {
        }
    }
}