using System;

namespace StreamStore.Sql
{
    internal class DefaultSqlExceptionHandler : ISqlExceptionHandler
    {
        public bool IsOptimisticConcurrencyException(Exception ex)
        {
            return false;
        }
    }
}
