using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using StreamStore.ExampleBase.Configuration;


namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    public static class HostApplicationBuilderExtension
    {
        public static RootCommand ConfigureExampleApplication(this HostApplicationBuilder builder, Action<ExampleApplicationConfigurator> configure)
        {
            builder.Configuration
                    .AddJsonFile("appsettings.json", true)
                    .AddJsonFile("appsettings.Development.json", true);

            var appConfigurator = new ExampleApplicationConfigurator();
            configure(appConfigurator);

            return appConfigurator.ConfigureCommand(builder);
        }
    }
}
