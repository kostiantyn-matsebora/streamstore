using System;
using Microsoft.Extensions.Hosting;

namespace StreamStore.ExampleBase.Configuration
{
    public sealed class StorageConfigurator
    {
        Action<IHostApplicationBuilder>? single;
        Action<IHostApplicationBuilder>? multi;

        public StorageConfigurator WithSingleMode(Action<IHostApplicationBuilder> configure)
        {
            single = configure;
            return this;
        }

        public StorageConfigurator WithMultitenancy(Action<IHostApplicationBuilder> configure)
        {
            multi = configure;
            return this;
        }

        internal void ConfigureStorage(IHostApplicationBuilder hostApplicationBuilder, StoreMode mode)
        {
            if (mode == StoreMode.Single)
            {
                single!(hostApplicationBuilder);
            }
            else
            {
                multi!(hostApplicationBuilder);
            }
        }
    }
}
