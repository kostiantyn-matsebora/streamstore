﻿using StreamStore.Testing.StreamStore;
using StreamStore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Stream;


namespace StreamStore.Tests.Enumerator
{
    public class EnumerableTestSuite : StreamStoreSuiteBase
    {
        readonly StreamReadingMode mode;

        public EnumerableTestSuite(): this(StreamReadingMode.Default)
        {
        }

        public EnumerableTestSuite(StreamReadingMode mode)
        {
            this.mode = mode;
        }

        internal IAsyncEnumerable<StreamEvent> CreateEnumerable(StreamReadingParameters parameters)
        {
            return new StreamEventEnumerable(parameters, Services.GetRequiredService<StreamEventEnumeratorFactory>());
        }

        protected override void ConfigureStreamStore(IStreamStoreConfigurator configurator)
        {
            configurator.WithReadingMode(mode);
            configurator.UseInMemoryDatabase();
        }
    }
}