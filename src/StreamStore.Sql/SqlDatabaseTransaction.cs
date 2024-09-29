using System.Threading.Tasks;
using System.Data.SqlClient;

namespace StreamStore.Sql
{
    partial class SqlEventUnitOfWork
    {
        public class SqlDatabaseTransaction : IDatabaseTransaction
        {
            private SqlConnection connection;
            private SqlTransaction transaction;
            public SqlDatabaseTransaction(SqlConnection connection, SqlTransaction transaction)
            {
                if (connection == null)
                    throw new System.ArgumentNullException(nameof(connection));
                this.connection = connection;
                if (transaction == null)
                    throw new System.ArgumentNullException(nameof(transaction));
                this.transaction = transaction;
            }

            public Task CommitAsync() => transaction.CommitAsync();
            public Task RollbackAsync() => transaction.RollbackAsync();
            public void Dispose()
            {
                transaction?.Dispose();
                connection?.Dispose();
                transaction = null!;
            }

            public void Enlist(SqlCommand context)
            {
                context.Connection = connection;
                context.Transaction = transaction;
            }
        }
    }
}
