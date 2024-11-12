using System;

using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.Configuration
{
    public abstract class DatabaseRegistratorBase: IStreamDatabaseRegistrator 
    {
        Action<IServiceCollection>? registrator;

        public void RegisterDependencies(Action<IServiceCollection> registrator)
        {
            this.registrator = registrator.ThrowIfNull(nameof(registrator));
        }


        public void Apply(IServiceCollection services)
        {
            if (registrator != null) registrator.Invoke(services);
            ApplyInternal(services);
            Validate(services);
        }

        protected abstract void ApplyInternal(IServiceCollection services);
        
        protected abstract void Validate(IServiceCollection services);

    }
}
