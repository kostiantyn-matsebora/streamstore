
using System;

namespace StreamStore
{
    public static class FuncExtension
    {
        public static T ThrowOriginalExceptionIfOccured<T>(this Func<T> action)
        {
            try
            {
                return action();
            }
            catch (AggregateException ex)
            {
                throw ex.GetFirstOriginalException();
            }
        }
    }
}
