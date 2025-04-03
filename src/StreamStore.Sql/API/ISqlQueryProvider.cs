
using StreamStore.Storage;

namespace StreamStore.Sql.API
{
    public interface ISqlQueryProvider
    {
        public string GetQuery(DatabaseOperation operation);
    }
}
