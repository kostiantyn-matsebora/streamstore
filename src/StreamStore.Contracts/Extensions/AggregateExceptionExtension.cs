using System;


namespace StreamStore
{
    public static class AggregateExceptionExtension
    {
        public static Exception GetFirstOriginalException(this AggregateException exception)
        {
            Exception realException = exception;
            var aggregateException = realException as AggregateException;

            if (aggregateException != null)
            {
                realException = aggregateException.Flatten().InnerException; 

                while (realException != null && realException.InnerException != null)
                {
                    realException = realException.InnerException;
                }
            }

            return realException ?? exception;
        }
    }
}
