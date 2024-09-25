using System;
using System.Collections.Generic;
using System.Text;

namespace StreamDB
{
    public abstract class StreamDBException: Exception
    {
        protected StreamDBException(string message) : base(message)
        {
        }
    }
}
