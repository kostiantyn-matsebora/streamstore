using System.Data.Common;


namespace StreamStore.SQL
{
    public interface IDbConnectionFactory
    {
        DbConnection GetConnection();
    }
}
