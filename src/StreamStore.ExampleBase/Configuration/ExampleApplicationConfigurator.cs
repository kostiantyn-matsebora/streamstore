﻿using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using StreamStore.ExampleBase.Progress;
using StreamStore.ExampleBase.Services.Multitenancy;
using StreamStore.ExampleBase.Services.SingleTenant;

namespace StreamStore.ExampleBase.Configuration
{
    [ExcludeFromCodeCoverage]
    public sealed class ExampleApplicationConfigurator
    {
        readonly RootCommandBuilder commandBuilder = new RootCommandBuilder(StoreMode.Single);
        readonly StorageConfiguratorRegistry configurators = new StorageConfiguratorRegistry();

        public ExampleApplicationConfigurator EnableMultitenancy()
        {
            commandBuilder.AddMode(StoreMode.Multitenancy);
            return this;
        }

        public ExampleApplicationConfigurator AddStorage<TEnum>(TEnum name, Action<StorageConfigurator> configure)
        {
            var storageName = Enum.GetName(typeof(TEnum), name).ToLower();
            commandBuilder.AddStorage(storageName);
            configure(configurators.GetOrAdd(storageName));
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


        static void RunHost(InvocationContext ctx, HostApplicationBuilder builder)
        {
            Console.WriteLine("Mode: {0}", ctx.Mode);
            Console.WriteLine("Storage backend: {0}", ctx.Storage);

            var host = builder.Build();
            host.Run();
        }


        void ConfigureHost(InvocationContext ctx, IHostApplicationBuilder builder)
        {
            ConfigureStorage(ctx, builder);
            ConfigureLogging(builder);
            ConfigureApplication(ctx, builder);
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

        void ConfigureStorage(InvocationContext ctx, IHostApplicationBuilder builder)
        {
            configurators
                .Get(ctx.Storage)
                .ConfigureStorage(builder, ctx.Mode);
        }
        static void ConfigureApplication(InvocationContext ctx, IHostApplicationBuilder builder)
        {
            switch (ctx.Mode)
            {
                case StoreMode.Single:
                    ConfigureSingleMode(builder);
                    break;
                case StoreMode.Multitenancy:
                    ConfigureMultitenancy(builder);
                    break;
            }
        }

        static void ConfigureSingleMode(IHostApplicationBuilder builder)
        {

            builder.Services
                .AddSingleton<WorkerRegistry>()
                .AddHostedService<Writer1>()
                .AddHostedService<Writer2>()
                .AddHostedService<Writer3>()
                .AddHostedService<Reader1>()
                .AddHostedService<Reader2>()
                .AddHostedService<Reader3>()
                .AddHostedService<ReaderToEnd1>()
                ;
        }

        static void ConfigureMultitenancy(IHostApplicationBuilder builder)
        {
            builder.Services
                .AddSingleton<WorkerRegistry>()
                .AddSingleton<TenantQueue>()
                .AddSingleton<TenantContextQueue>()
                .AddHostedService<TenantOne>()
                .AddHostedService<TenantTwo>()
                .AddHostedService<TenantThree>()
                ;
        }
    }
}
