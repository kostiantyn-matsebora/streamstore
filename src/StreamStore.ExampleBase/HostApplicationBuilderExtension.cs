using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    public static class HostApplicationBuilderExtension
    {

        public static IHostApplicationBuilder ConfigureExampleApplication(this IHostApplicationBuilder builder)
        {
            builder.Logging
             .AddSimpleConsole(configure =>
             {
                 configure.SingleLine = true;
                 configure.ColorBehavior = LoggerColorBehavior.Enabled;
                 configure.IncludeScopes = true;
             });


            builder.Services
                .AddHostedService<Writer1>()
                .AddHostedService<Writer2>()
                .AddHostedService<Writer3>()
                .AddHostedService<Reader1>()
                .AddHostedService<Reader2>()
                .AddHostedService<Reader3>()
                .AddHostedService<ReaderToEnd1>();
            return builder;
        }
    }
}
