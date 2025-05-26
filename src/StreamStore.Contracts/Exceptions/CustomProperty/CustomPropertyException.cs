using System;

namespace StreamStore.Exceptions { 
    public abstract class CustomPropertyException : StreamStoreException
    {
        protected CustomPropertyException(string message)
            : base(message)
        {
        }
        protected CustomPropertyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
