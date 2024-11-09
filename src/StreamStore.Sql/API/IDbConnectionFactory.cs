using System.Data.Common;


namespace StreamStore.Sql.API
{
    public interface IDbConnectionFactory
    {
        DbConnection GetConnection();
    }
}
