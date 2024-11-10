using System;
using StreamStore.Sql.API;

namespace StreamStore.Sql.Postgres
{
    internal class PostgresExceptionHandler : ISqlExceptionHandler
    {
        public bool IsOptimisticConcurrencyException(Exception ex)
        {
            return false;
        }
    }
}
