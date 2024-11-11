using System;
using StreamStore.Sql.API;

namespace StreamStore.Sql.PostgreSql
{
    internal class PostgresExceptionHandler : ISqlExceptionHandler
    {
        public bool IsOptimisticConcurrencyException(Exception ex)
        {
            var exception = ex as Npgsql.PostgresException;
            if (exception == null)
            {
                return false;
            }

            return exception.SqlState == "23505";
        }
    }
}
