using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    public sealed class ExampleApplicationConfigurator
    {
        readonly RootCommandBuilder commandBuilder = new RootCommandBuilder(StoreMode.Single);
        readonly DatabaseConfiguratorRegistry configurators = new DatabaseConfiguratorRegistry();


        public ExampleApplicationConfigurator EnableMultitenancy()
        {
            commandBuilder.AddMode(StoreMode.MultiTenant);
            return this;
        }

        public ExampleApplicationConfigurator AddDatabase<TEnum>(TEnum name, Action<DatabaseConfigurator> configure)
        {
            var databaseName = Enum.GetName(typeof(TEnum), name).ToLower();
            commandBuilder.AddDatabase(databaseName);
            configure(configurators.GetOrAdd(databaseName));
            return this;
        }

        public RootCommand ConfigureCommand(HostApplicationBuilder builder)
        {
            return commandBuilder.Build(ctx =>
            {
                ConfigureHost(ctx, builder);
                RunHost(ctx, builder);
            });
        }


        void RunHost(InvocationContext ctx, HostApplicationBuilder builder)
        {
            Console.WriteLine("Mode: {0}", ctx.Mode);
            Console.WriteLine("Database backend: {0}", ctx.Database);

            var host = builder.Build();
            host.Run();
        }

        
        void ConfigureHost(InvocationContext ctx, IHostApplicationBuilder builder)
        {            
            ConfigureDatabase(ctx, builder);
            ConfigureLogging(builder);
            ConfigureServices(builder);
        }

        void ConfigureDatabase(InvocationContext ctx, IHostApplicationBuilder builder)
        {
            configurators
                .Get(ctx.Database)
                .ConfigureDatabase(builder, ctx.Mode);
        }

        static void ConfigureServices(IHostApplicationBuilder builder)
        {
            builder.Services
                .AddHostedService<Writer1>()
                .AddHostedService<Writer2>()
                .AddHostedService<Writer3>()
                .AddHostedService<Reader1>()
                .AddHostedService<Reader2>()
                .AddHostedService<Reader3>()
                .AddHostedService<ReaderToEnd1>();
        }

        static void ConfigureLogging(IHostApplicationBuilder builder)
        {
            builder.Logging
                 .AddSimpleConsole(configure =>
                 {
                     configure.SingleLine = true;
                     configure.ColorBehavior = LoggerColorBehavior.Enabled;
                     configure.IncludeScopes = true;
                 });
        }
    }
}
