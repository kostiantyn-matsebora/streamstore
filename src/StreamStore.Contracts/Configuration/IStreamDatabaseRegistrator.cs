using Microsoft.Extensions.DependencyInjection;
using System;

namespace StreamStore
{
    public interface IStreamDatabaseRegistrator
    {
        public void RegisterDependencies(Action<IServiceCollection> registrator);
        public void Apply(IServiceCollection services);
    }

    public interface ISingleTenantDatabaseRegistrator: IStreamDatabaseRegistrator
    {
        ISingleTenantDatabaseRegistrator RegisterDatabase<TDatabase>() where TDatabase : IStreamDatabase;
    }

    public interface IMultitenantDatabaseRegistrator : IStreamDatabaseRegistrator
    {
    }
}
