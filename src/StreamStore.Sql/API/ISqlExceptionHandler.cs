using System;

namespace StreamStore.Sql.API
{
    public interface ISqlExceptionHandler
    {
        void HandleException(Exception ex, Id streamId);

    }
}
