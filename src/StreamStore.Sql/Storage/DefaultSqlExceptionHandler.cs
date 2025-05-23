using System;
using StreamStore.Sql.API;

namespace StreamStore.Sql.Storage
{
    internal class DefaultSqlExceptionHandler : ISqlExceptionHandler
    {
        public void HandleException(Exception ex, Id streamId)
        {
        }
    }
}
