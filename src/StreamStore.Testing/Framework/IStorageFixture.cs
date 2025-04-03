namespace StreamStore.Testing.Framework
{
    public interface IStorageFixture
    {
        MemoryStorage Container { get; }
        void ConfigureStorage(ISingleTenantConfigurator configurator);

        bool IsStorageReady { get; }
    }
}
