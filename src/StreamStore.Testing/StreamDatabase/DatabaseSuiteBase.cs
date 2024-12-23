﻿
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing.Framework;



namespace StreamStore.Testing.StreamDatabase
{
    public abstract class DatabaseSuiteBase : TestSuiteBase
    {
        readonly MemoryDatabase container = new MemoryDatabase();

        public IStreamDatabase StreamDatabase => Services.GetRequiredService<IStreamDatabase>();

        public virtual MemoryDatabase Container => container;

        protected override sealed void RegisterServices(IServiceCollection services)
        {
            new StreamStoreConfigurator()
                .WithSingleDatabase(ConfigureDatabase)
                .Configure(services);
        }

        protected abstract void ConfigureDatabase(ISingleTenantConfigurator configurator);

        protected override void SetUp()
        {
            Container.CopyTo(StreamDatabase);
        }
    }
}
