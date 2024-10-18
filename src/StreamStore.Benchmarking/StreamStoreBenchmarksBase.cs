using AutoFixture;
using StreamStore.InMemory;
using StreamStore.Serialization;
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
        protected readonly StreamStore store;

        public StreamStoreBenchmarksBase()
        {
                var fixture = new Fixture();

                events = fixture.CreateMany<Event>(10).ToArray();
                store = new StreamStore(new InMemoryStreamDatabase(), new NewtonsoftEventSerializer(TypeRegistry.CreateAndInitialize()));

                streamIds = Enumerable
                    .Range(0, 1000)
                    .Select(i => Guid.NewGuid().ToString())
                    .ToArray();

                Parallel.ForEach(streamIds, streamId =>
                {
                    store
                        .OpenStreamAsync(streamId, CancellationToken.None)
                        .AddRangeAsync(events, CancellationToken.None)
                        .SaveChangesAsync(CancellationToken.None).Wait();
                });
        }
    }
}
