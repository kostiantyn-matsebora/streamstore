using System;
using StreamStore.Sql.API;

namespace StreamStore.Sql.Database
{
    internal class DefaultSqlExceptionHandler : ISqlExceptionHandler
    {
        public bool IsOptimisticConcurrencyException(Exception ex)
        {
            return false;
        }
    }
}
