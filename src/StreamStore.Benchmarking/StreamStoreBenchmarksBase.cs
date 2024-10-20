using AutoFixture;
using Dapper.Extensions;
using Dapper.Extensions.Factory;
using StreamStore.InMemory;
using StreamStore.Serialization;
using StreamStore.Sql.Tests.Sqlite;
using StreamStore.SQL.Sqlite;
using System;
using System.Linq;
using System.Threading;


namespace StreamStore.Benchmarking
{
    public class StreamStoreBenchmarksBase
    {
        protected readonly Event[] events;
        protected readonly string[] streamIds;
        protected readonly StreamStore inMemoryStore;
        readonly NewtonsoftEventSerializer serializer;
        readonly SqliteTestSuite suite;

        public StreamStoreBenchmarksBase()
        {
            var fixture = new Fixture();
            fixture.Customize<Event>(composer => composer.Do(e => e.Id = Guid.NewGuid().ToString()));
            events = fixture.CreateMany<Event>(100).ToArray();

            serializer = CreateSerializer();
            suite = new SqliteTestSuite();
            inMemoryStore = CreateInMemoryStore();

            streamIds = GenerateStreamIds();
        }

        protected void FillDatabase()
        {
            InsertStreamsToStore(inMemoryStore);
            DapperFactory.Step(dapper =>
            {
                var store = CreateSqliteStore(dapper);
                InsertStreamsToStore(store);
            });
        }

        string[] GenerateStreamIds()
        {
            return Enumerable
                .Range(0, 100)
                .Select(i => Guid.NewGuid().ToString())
                .ToArray();
        }

        static NewtonsoftEventSerializer CreateSerializer()
        {
            var typeRegistry = TypeRegistry.CreateAndInitialize();
            var serializer = new NewtonsoftEventSerializer(typeRegistry);
            return serializer;
        }

        StreamStore CreateInMemoryStore()
        {
            return new StreamStore(new InMemoryStreamDatabase(), serializer);
        }

        protected StreamStore CreateSqliteStore(IDapper dapper)
        {
            return new StreamStore(new SqliteStreamDatabase(dapper, suite.Configuration), serializer);
        }

        void InsertStreamsToStore(StreamStore store)
        {
            foreach (var streamId in streamIds)
            {
                store
                  .OpenStreamAsync(streamId, CancellationToken.None)
                  .AddRangeAsync(events, CancellationToken.None)
                  .SaveChangesAsync(CancellationToken.None).Wait();
            }
        }
    }
}
