namespace StreamStore.Sql.Tests.Database
{
    public interface ITestDatabase
    {
        string ConnectionString { get; }

        bool EnsureExists();
    }
}
