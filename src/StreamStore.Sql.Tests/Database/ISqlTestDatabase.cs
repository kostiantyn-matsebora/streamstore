using StreamStore.Testing.Framework;

namespace StreamStore.Sql.Tests.Database
{
    public interface ISqlTestDatabase: ITestDatabase
    {
        string ConnectionString { get; }
    }
}
