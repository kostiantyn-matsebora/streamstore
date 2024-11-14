using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    public sealed class ExampleApplicationConfigurator
    {
        bool multitenancyEnabled;

        readonly List<DatabaseConfigurator> configurators = new List<DatabaseConfigurator>();

        public ExampleApplicationConfigurator EnableMultitenancy()
        {
            multitenancyEnabled = true;
            return this;
        }

        public ExampleApplicationConfigurator AddDatabase<TEnum>(TEnum name, Action<DatabaseConfigurator> configure)
        {
            var configurator = new DatabaseConfigurator();
            configurator.WithName(name);
            configure(configurator);
            configurators.Add(configurator);
            return this;
        }

        public RootCommand ConfigureHost(HostApplicationBuilder hostApplicationBuilder)
        {
            if (configurators.Count == 0)
            {
                throw new InvalidOperationException("At least one database configuration must be added.");
            }

            return ConfigureCommandLine(hostApplicationBuilder);
        }


        RootCommand ConfigureCommandLine(HostApplicationBuilder builder)
        {
            var rootCommand = new RootCommand("Sample application for StreamStore");

            Option<StoreMode> modeOption = CreateModeOption();

            Option<string> databaseOption = CreateDatabaseOption();

            rootCommand.AddOption(databaseOption);
            rootCommand.AddOption(modeOption);
            rootCommand.SetHandler((mode, database) =>
            {
                Console.WriteLine("Mode: {0}", mode);
                Console.WriteLine("Database backend: {0}", database);

                ConfigureHost(builder, mode, database);
                var host = builder.Build();
                host.Run();
            }, modeOption, databaseOption);
            return rootCommand;
        }

        void ConfigureHost(IHostApplicationBuilder builder, StoreMode mode, string database)
        {
            var configurator = configurators.FirstOrDefault(c => c.Name == database);

            if (configurator == null)
            {
                throw new InvalidOperationException($"Database provider {database} not found.");
            }
            configurator.ConfigureDatabase(builder, mode);
            ConfigureLogging(builder);
            ConfigureServices(builder);
        }

        private static void ConfigureServices(IHostApplicationBuilder builder)
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

        private static void ConfigureLogging(IHostApplicationBuilder builder)
        {
            builder.Logging
                 .AddSimpleConsole(configure =>
                 {
                     configure.SingleLine = true;
                     configure.ColorBehavior = LoggerColorBehavior.Enabled;
                     configure.IncludeScopes = true;
                 });
        }

        Option<string> CreateDatabaseOption()
        {
            var firstDatabase = configurators.First();
            var databases = ListDatabases(configurators);

            var databaseOption = new Option<string>(
                name: "--database",
                getDefaultValue: () => firstDatabase.Name!,
                $"Database provider, possible values: {ListDatabases(configurators)}. Default: {firstDatabase.Name}");
            return databaseOption;
        }

        Option<StoreMode> CreateModeOption()
        {
            var modeOption = new Option<StoreMode>(
                    name: "--mode",
                    getDefaultValue: () => StoreMode.Single,
                    $"Store mode, possible values: {ListModes()}. Default: single");
            return modeOption;
        }

        string ListDatabases(IEnumerable<DatabaseConfigurator> configurators)
        {
            return string.Join(",", configurators.Select(c => c.Name));
        }

        string ListModes()
        {
            return multitenancyEnabled ?
                string.Join(",", Enum.GetNames(typeof(StoreMode))).ToLower() :
                StoreMode.Single.ToString().ToLower();
        }
    }
}
