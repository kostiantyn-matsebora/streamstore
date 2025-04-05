using StreamStore.Testing.Framework;

namespace StreamStore.Sql.Tests.Storage
{
    public interface ISqlTestStorage: ITestStorage
    {
        string ConnectionString { get; }
    }
}
