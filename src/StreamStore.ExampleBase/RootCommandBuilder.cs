using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using StreamStore.Extensions;

namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    class RootCommandBuilder
    {
        readonly HashSet<string> modes = new HashSet<string>();
        readonly HashSet<string> databases = new HashSet<string>();

        public RootCommandBuilder(params StoreMode[] modes)
        {
            if (modes != null) Array.ForEach(modes, RegisterMode);
        }

        public RootCommandBuilder AddDatabase(string database)
        {
            databases.Add(database);
            return this;
        }

        public RootCommandBuilder AddMode(StoreMode mode)
        {
            RegisterMode(mode);
            return this;
        }

        public RootCommand Build(Action<InvocationContext> command)
        {
            var rootCommand = new RootCommand("Sample application for StreamStore");

            Option<string> modeOption = CreateModeOption();
            Option<string> databaseOption = CreateDatabaseOption();

            rootCommand.AddOption(databaseOption);
            rootCommand.AddOption(modeOption);
            rootCommand.SetHandler((mode, database) =>
                command(new InvocationContext(mode.ToEnum<StoreMode>(), database)),
                modeOption, 
                databaseOption);

            return rootCommand;
        }

        Option<string> CreateDatabaseOption()
        {
            var firstDatabase = databases.First();

            var databaseOption = new Option<string>(
                name: "--database",
                getDefaultValue: () => firstDatabase,
                $"Database backend, possible values: {databases.CommaSeparated()}.");
            return databaseOption;
        }

        Option<string> CreateModeOption()
        {
            var firstMode = modes.First();
            var modeOption = new Option<string>(
                    name: "--mode",
                    getDefaultValue: () => firstMode,
                    $"Store mode, possible values: {modes.CommaSeparated()}.");
            return modeOption;
        }

        void RegisterMode(StoreMode mode)
        {
            modes.Add(mode.ToLowerString());
        }
    }
}
