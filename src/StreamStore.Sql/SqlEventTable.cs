
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


        public IEventUnitOfWork CreateUnitOfWork(string streamId, int expectedStreamVersion, IDatabaseTransaction<SqlCommand>? transaction = null)
        {
            if (transaction == null)
                return new SqlEventUnitOfWork(connectionString, tableName);

            return new SqlEventUnitOfWork(transaction!, tableName);
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
    }
}
