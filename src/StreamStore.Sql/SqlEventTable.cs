
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;

namespace StreamStore.Sql
{
    public class SqlEventTable : IEventTable
    {
        private readonly string connectionString;
        private readonly string tableName;

        public SqlEventTable(string connectionString, string tableName = "Events")
        {
            this.connectionString = connectionString;
            this.tableName = tableName;
        }

        public async Task DeleteAsync(string streamId, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);

            var command = new SqlCommand(string.Format("DELETE FROM {0} WHERE StreamId = @StreamId", tableName), connection);
            command.Parameters.Add(new SqlParameter("@StreamId", SqlDbType.NVarChar) { Value = streamId });
            command.ExecuteNonQuery();
        }

        public async Task<StreamRecord?> FindAsync(string streamId, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);
            var command = new SqlCommand(string.Format("SELECT * FROM {0} WHERE StreamId = @StreamId ORDER BY Revision", tableName), connection);
            command.Parameters.Add(new SqlParameter("@StreamId", SqlDbType.NVarChar) { Value = streamId });
            SqlDataReader sqlDataReader = await command.ExecuteReaderAsync();

            var events = new List<EventRecord>();
            while (await sqlDataReader.ReadAsync())
            {
                events.Add(new EventRecord
                {
                    Id = sqlDataReader["Id"].ToString(),
                    Data = sqlDataReader["Data"].ToString(),
                    Timestamp = (DateTime)sqlDataReader["Timestamp"],
                    Revision = (int)sqlDataReader["Revision"]
                });
            }

            if (events.Count == 0)
                return null;

            return new StreamRecord(streamId, events.ToArray());
        }

        public async Task<StreamMetadataRecord?> FindMetadataAsync(string streamId, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);
            var command = new SqlCommand(string.Format("SELECT Id, Timestamp, Revision FROM {0} WHERE StreamId = @StreamId ORDER BY Revision", tableName), connection);
            command.Parameters.Add(new SqlParameter("@StreamId", SqlDbType.NVarChar) { Value = streamId });
            SqlDataReader sqlDataReader = await command.ExecuteReaderAsync();

            var events = new List<EventMetadataRecord>();
            while (await sqlDataReader.ReadAsync())
            {
                events.Add(new EventMetadataRecord
                {
                    Id = sqlDataReader["Id"].ToString(),
                    Timestamp = (DateTime)sqlDataReader["Timestamp"],
                    Revision = (int)sqlDataReader["Revision"]
                });
            }

            if (events.Count == 0)
                return null;

            return new StreamMetadataRecord(streamId, events.ToArray());
        }

        public async Task InsertAsync(string streamId, IEnumerable<EventRecord> events, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            foreach (var @event in events)
            {
                var command = new SqlCommand(string.Format("INSERT INTO {0} (StreamId, Id, Data, Timestamp, Revision) VALUES (@StreamId, @Id, @Data, @Timestamp, @Revision)", tableName), connection, transaction);
                command.Parameters.Add(new SqlParameter("@StreamId", SqlDbType.NVarChar) { Value = streamId });
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.NVarChar) { Value = @event.Id });
                command.Parameters.Add(new SqlParameter("@Data", SqlDbType.NVarChar) { Value = @event.Data });
                command.Parameters.Add(new SqlParameter("@Timestamp", SqlDbType.DateTime) { Value = @event.Timestamp });
                command.Parameters.Add(new SqlParameter("@Revision", SqlDbType.Int) { Value = @event.Revision });
                await command.ExecuteNonQueryAsync();
            }

            transaction.Commit();
        }
    }
}
