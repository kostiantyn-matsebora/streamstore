using System;
using Microsoft.Extensions.Hosting;

namespace StreamStore.ExampleBase
{
    public sealed class DatabaseConfigurator
    {
        Action<IHostApplicationBuilder>? single;
        Action<IHostApplicationBuilder>? multi;

        internal string? Name { get; private set; }

        internal DatabaseConfigurator WithName<TEnum>(TEnum name)
        {
            Name = Enum.GetName(typeof(TEnum),name).ToLower();
            return this;
        }

        public DatabaseConfigurator WithSingleMode(Action<IHostApplicationBuilder> configure)
        {
            this.single = configure;
            return this;
        }

        public DatabaseConfigurator WithMultitenancy(Action<IHostApplicationBuilder> configure)
        {
            this.multi = configure;
            return this;
        }

        internal void ConfigureDatabase(IHostApplicationBuilder hostApplicationBuilder, StoreMode mode)
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
