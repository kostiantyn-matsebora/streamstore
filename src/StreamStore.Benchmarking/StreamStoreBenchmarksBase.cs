using AutoFixture;
using Dapper.Extensions;
using Dapper.Extensions.Factory;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Tests.Sqlite;
using StreamStore.Tests.InMemory;
using System;
using System.Linq;
using System.Threading;


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

        protected void FillDatabase()
        {
            InsertStreamsToStore(inMemoryStore);
            var store = GetSqliteStore();
            InsertStreamsToStore(store);
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

        void InsertStreamsToStore(IStreamStore store)
        {
            foreach (var streamId in streamIds)
            {
                var stream = store
                  .OpenAsync(streamId, CancellationToken.None)
                  .BeginWriteAsync(CancellationToken.None);
                  
            }
        }
    }
}
