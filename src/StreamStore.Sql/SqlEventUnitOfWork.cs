
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;

namespace StreamStore.Sql
{
    partial class SqlEventUnitOfWork : IEventUnitOfWork
    {
        IDatabaseTransaction<SqlCommand>? transaction;
        SqlConnection? connection;
        List<EventRecord> events = new List<EventRecord>();
        string? streamId;

        string? connectionString;
        string tableName;

        public SqlEventUnitOfWork(string connectionString,  string tableName = "Events")
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            this.connectionString = connectionString;
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));
            this.tableName = tableName;
        }

        public SqlEventUnitOfWork(IDatabaseTransaction<SqlCommand> transaction, string tableName = "Events")
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));
            this.transaction = transaction;
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));
            this.tableName = tableName;
        }

        public Task BeginTransactionAsync(string streamId, int expectedRevision, CancellationToken cancellationToken)
        {
            events = new List<EventRecord>();
            this.streamId = streamId;
            return Task.CompletedTask;
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (transaction == null)
                {
                    throw new InvalidOperationException("Transaction not started");
                }

                foreach (var @event in events)
                {
                    var command = new SqlCommand(string.Format("INSERT INTO {0} (StreamId, Id, Data, Timestamp, Revision) VALUES (@StreamId, @Id, @Data, @Timestamp, @Revision)", tableName), connection);

                    command.Parameters.Add(new SqlParameter("@StreamId", SqlDbType.NVarChar) { Value = streamId });
                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.NVarChar) { Value = @event.Id });
                    command.Parameters.Add(new SqlParameter("@Data", SqlDbType.NVarChar) { Value = @event.Data });
                    command.Parameters.Add(new SqlParameter("@Timestamp", SqlDbType.DateTime) { Value = @event.Timestamp });
                    command.Parameters.Add(new SqlParameter("@Revision", SqlDbType.Int) { Value = @event.Revision });

                    transaction!.Enlist(command);
                    await command.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch
            {
                transaction?.RollbackAsync();
                throw;
            }
        }

        public void Dispose()
        {
            transaction?.Dispose();
            connection?.Dispose();
        }

        public Task Add(Id eventId, int revision, DateTime timestamp, string data)
        {
           events.Add(new EventRecord { Id = eventId, Revision = revision, Timestamp = timestamp, Data = data });
           return Task.CompletedTask;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (transaction != null)
                throw new InvalidOperationException("Transaction already started");

            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            transaction = new SqlDatabaseTransaction(connection!.BeginTransaction());
        }
    }


}
