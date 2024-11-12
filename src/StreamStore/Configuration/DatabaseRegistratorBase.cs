using System;

using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.Configuration
{
    public abstract class DatabaseRegistratorBase: IStreamDatabaseRegistrator 
    {
        Action<IServiceCollection>? configurator;

        public void ConfigureWith(Action<IServiceCollection> configurator)
        {
            this.configurator = configurator.ThrowIfNull(nameof(configurator));
        }


        public void Register(IServiceCollection services, StreamStoreConfiguration configuration)
        {
            if (configurator == null)
            {
                throw new InvalidOperationException("Database backend is not set");
            }

            configurator.Invoke(services);
            Validate(services);
        }

        protected abstract void Validate(IServiceCollection services);

    }
}
