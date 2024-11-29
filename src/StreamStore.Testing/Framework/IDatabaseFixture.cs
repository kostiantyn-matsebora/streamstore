namespace StreamStore.Testing.Framework
{
    public interface IDatabaseFixture
    {
        MemoryDatabase Container { get; }
        void ConfigureDatabase(ISingleTenantConfigurator configurator);

        bool IsDatabaseReady { get; }
    }
}
