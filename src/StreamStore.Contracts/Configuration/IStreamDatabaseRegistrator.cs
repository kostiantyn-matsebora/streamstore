using Microsoft.Extensions.DependencyInjection;
using System;

namespace StreamStore
{
    public interface IStreamDatabaseRegistrator
    {
        public bool MultiTenancyEnabled { get; }
        public void ConfigureWith(Action<IServiceCollection> configurator);
        public void Register(IServiceCollection services);
    }
}
