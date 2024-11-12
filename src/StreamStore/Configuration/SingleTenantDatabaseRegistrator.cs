using Microsoft.Extensions.DependencyInjection;
using StreamStore.Configuration;
using System;
using System.Linq;

namespace StreamStore
{
    public class SingleTenantDatabaseRegistrator : DatabaseRegistratorBase, ISingleTenantDatabaseRegistrator
    {
        Action<IServiceCollection>? registerDatabase;

        public ISingleTenantDatabaseRegistrator RegisterDatabase<TDatabase>() where TDatabase : IStreamDatabase
        {
            registerDatabase = services =>
            {
                services.AddSingleton(typeof(IStreamDatabase), typeof(TDatabase));
                services.AddSingleton(typeof(IStreamReader), typeof(TDatabase));
            };

            return this;
        }

        protected override void ApplyInternal(IServiceCollection services)
        {
            if (registerDatabase == null) throw new InvalidOperationException("Database registration is not set");

            registerDatabase(services);
        }

        protected override void Validate(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(IStreamDatabase)))
            {
                throw new InvalidOperationException("Database backend (IStreamDatabase) is not registered");
            }

            if (!services.Any(s => s.ServiceType == typeof(IStreamReader)))
            {
                throw new InvalidOperationException("Database backend (IStreamReader) is not registered");
            }
        }
    }
}
