using System;

namespace StreamStore.Sql.API
{
    public interface ISqlExceptionHandler
    {
        bool IsOptimisticConcurrencyException(Exception ex);
    }
}
