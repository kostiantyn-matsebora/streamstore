using Microsoft.Extensions.DependencyInjection;
using System;

namespace StreamStore
{
    public interface IStreamDatabaseRegistrator
    {
        public void ConfigureWith(Action<IServiceCollection> configurator);
        public void Register(IServiceCollection services, StreamStoreConfiguration configuration);
    }

    public interface ISingleTenantDatabaseRegistrator: IStreamDatabaseRegistrator
    {
    }

    public interface IMultitenantDatabaseRegistrator : IStreamDatabaseRegistrator
    {
    }
}
