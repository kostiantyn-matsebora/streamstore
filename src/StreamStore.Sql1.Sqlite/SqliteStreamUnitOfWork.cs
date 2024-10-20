using System;
using System.Data.SQLite;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Dapper.Extensions;
using StreamStore.Exceptions;


namespace StreamStore.SQL.Sqlite
{
    internal class SqliteStreamUnitOfWork : StreamUnitOfWorkBase
    {
        private readonly IDapper dapper;
        readonly SqliteDatabaseConfiguration configuration;
  
        public SqliteStreamUnitOfWork(Id streamId, Revision expectedRevision, StreamRecord? existing, SqliteDatabaseConfiguration configuration, IDapper dapper) :
            base(streamId, expectedRevision, existing)
        {
            this.dapper = dapper ?? throw new ArgumentNullException(nameof(dapper));

            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override async Task SaveChangesAsync(EventRecordCollection uncommited, CancellationToken token)
        {
            var sql = $"INSERT INTO {configuration.FullTableName} (Id, StreamId, Revision, Timestamp, Data) VALUES (@Id, @StreamId, @Revision, @Timestamp, @Data)";
            try
            {
                    using (var transaction = dapper.BeginTransaction())
                    {
                       
                        await dapper.ExecuteAsync(sql, uncommited.ToEntityArray(streamId));
                        dapper.CommitTransaction();
                    }
            }
            catch (SQLiteException e)
            {
                if (e.ErrorCode != 19)
                {
                    throw;
                }

                throw new OptimisticConcurrencyException(expectedRevision, GetActualRevision(), streamId);
            }
        }

        int GetActualRevision()
        {
            var sql = $"SELECT MAX(Revision) FROM {configuration.FullTableName} WHERE StreamId = @StreamId";
            return dapper.ExecuteScalar<int>(sql, new { StreamId = (string)streamId });
        }
    }
}
