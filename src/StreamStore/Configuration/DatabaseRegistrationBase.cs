using System;

using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.Configuration
{
    internal abstract class DatabaseRegistrationBase: IStreamDatabaseRegistrator 
    {
        Action<IServiceCollection>? configurator;

        public abstract bool MultiTenancyEnabled { get; }

        public void ConfigureWith(Action<IServiceCollection> configurator)
        {
            this.configurator = configurator.ThrowIfNull(nameof(configurator));
        }


        public void Register(IServiceCollection services)
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
