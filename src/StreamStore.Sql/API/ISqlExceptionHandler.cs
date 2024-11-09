using System;

namespace StreamStore.Sql
{
    public interface ISqlExceptionHandler
    {
        bool IsOptimisticConcurrencyException(Exception ex);
    }
}
