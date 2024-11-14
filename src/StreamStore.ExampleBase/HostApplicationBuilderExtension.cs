using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;


namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    public static class HostApplicationBuilderExtension
    {
        public static RootCommand ConfigureExampleApplication(this HostApplicationBuilder builder, Action<ExampleApplicationConfigurator> configure)
        {
            var appConfigurator = new ExampleApplicationConfigurator();
            configure(appConfigurator);

            return appConfigurator.ConfigureCommand(builder);
        }
    }
}
