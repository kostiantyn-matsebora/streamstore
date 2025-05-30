using System;
using System.Data.SQLite;
using StreamStore.Exceptions;
using StreamStore.Exceptions.Appending;
using StreamStore.Sql.API;


namespace StreamStore.Sql.Sqlite
{
    internal class SqliteExceptionHandler : ISqlExceptionHandler
    {
        public void HandleException(Exception ex, Id streamId)
        {
            var sqliteException = ex as SQLiteException;
            if (sqliteException == null || sqliteException.ErrorCode == 19)
            {
                if (sqliteException!.Message.Contains(".Revision"))
                {
                    throw new RevisionAlreadyExistsException(streamId);
                }

                if (sqliteException.Message.Contains(".Id"))
                {
                    throw new EventAlreadyExistsException(streamId);
                }
            }

        }
    }
}
