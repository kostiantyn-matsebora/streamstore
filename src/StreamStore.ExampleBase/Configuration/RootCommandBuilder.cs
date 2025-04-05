using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using StreamStore.Extensions;

namespace StreamStore.ExampleBase.Configuration
{
    [ExcludeFromCodeCoverage]
    class RootCommandBuilder
    {
        readonly HashSet<string> modes = new HashSet<string>();
        readonly HashSet<string> storages = new HashSet<string>();

        public RootCommandBuilder(params StoreMode[] modes)
        {
            if (modes != null) Array.ForEach(modes, RegisterMode);
        }

        public RootCommandBuilder AddStorage(string storage)
        {
            storages.Add(storage);
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
            Option<string> storageOption = CreateStorageOption();

            rootCommand.AddOption(storageOption);
            rootCommand.AddOption(modeOption);
            rootCommand.SetHandler((mode, storage) =>
                command(new InvocationContext(mode.ToEnum<StoreMode>(), storage)),
                modeOption,
                storageOption);

            return rootCommand;
        }

        Option<string> CreateStorageOption()
        {
            var firstStorage = storages.First();

            var storageOption = new Option<string>(
                name: "--storage",
                getDefaultValue: () => firstStorage,
                $"Storage backend, possible values: {storages.CommaSeparated()}.");
            return storageOption;
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
