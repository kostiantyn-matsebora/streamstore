using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Tests.Sqlite;
using StreamStore.Tests.InMemory;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace StreamStore.Benchmarking
{
    public class StreamStoreBenchmarksBase
    {
        protected readonly Event[] events;
        protected readonly string[] streamIds;
        protected readonly IStreamStore inMemoryStore;
        readonly SqliteTestSuite sqliteSuite;
        readonly InMemoryTestSuite inMemorySuite;

        public StreamStoreBenchmarksBase()
        {
            var fixture = new Fixture();
            fixture.Customize<Event>(composer => composer.Do(e => e.Id = Guid.NewGuid().ToString()));

            events = fixture.CreateMany<Event>(100).ToArray();

            sqliteSuite = new SqliteTestSuite();
            sqliteSuite.SetUpSuite().Wait();

            inMemorySuite = new InMemoryTestSuite();
            inMemorySuite.SetUpSuite().Wait();

            inMemoryStore = GetInMemoryStore();
            streamIds = GenerateStreamIds();
        }

        protected async Task FillDatabaseAsync()
        {
            await InsertStreamsToStore(inMemoryStore);
            var store = GetSqliteStore();
            await InsertStreamsToStore(store);
        }

        static string[] GenerateStreamIds()
        {
            return Enumerable
                .Range(0, 100)
                .Select(i => Guid.NewGuid().ToString())
                .ToArray();
        }

        IStreamStore GetInMemoryStore()
        {
            return inMemorySuite.Services.GetRequiredService<IStreamStore>();
        }

        protected IStreamStore GetSqliteStore()
        {
           return sqliteSuite.Services.GetRequiredService<IStreamStore>();
        }

        async Task InsertStreamsToStore(IStreamStore store)
        {
            foreach (var streamId in streamIds)
            {
                await store
                  .BeginWriteAsync(streamId, CancellationToken.None)
                  .AddRangeAsync(events);
            }
        }
    }
}
