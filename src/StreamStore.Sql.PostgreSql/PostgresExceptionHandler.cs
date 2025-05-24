using System;
using StreamStore.Exceptions;
using StreamStore.Sql.API;

namespace StreamStore.Sql.PostgreSql
{
    internal class PostgresExceptionHandler : ISqlExceptionHandler
    {
        public void HandleException(Exception ex, Id streamId)
        {
            var exception = ex as Npgsql.PostgresException;
            if (exception == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(exception.ConstraintName)) return;

            if (exception.SqlState == "23505")
            {
                if (exception.ConstraintName.Contains("pkey")) throw new EventAlreadyExistsException(streamId);
                if (exception.ConstraintName.Contains("revision")) throw new RevisionAlreadyExistsException(streamId);
            }
        }
    }
}
