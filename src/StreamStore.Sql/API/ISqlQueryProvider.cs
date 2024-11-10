
namespace StreamStore.Sql.API
{
    public interface ISqlQueryProvider
    {
        public string GetQuery(SqlQueryType queryType);
    }
}
