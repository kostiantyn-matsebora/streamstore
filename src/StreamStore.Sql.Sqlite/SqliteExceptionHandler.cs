﻿using System;
using System.Data.SQLite;


namespace StreamStore.Sql.Sqlite
{
    internal class SqliteExceptionHandler : ISqlExceptionHandler
    {
        public bool IsOptimisticConcurrencyException(Exception ex)
        {
            var sqliteException = ex as SQLiteException;
            if (sqliteException == null || sqliteException.ErrorCode != 19)
            {
                return false;
            }
            return true;
        }
    }
}