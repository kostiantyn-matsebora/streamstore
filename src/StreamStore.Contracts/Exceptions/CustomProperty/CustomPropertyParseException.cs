using System;

namespace StreamStore.Exceptions
{
    
    public sealed class CustomPropertyParseException : CustomPropertyException
    {
        public CustomPropertyParseException(string key, string value, Exception innerException)
           : base($"Failed to parse custom property '{key}' with value '{value}' due to '{innerException.Message}'", innerException)
        {
        }
    }
}