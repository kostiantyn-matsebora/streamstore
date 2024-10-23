using System.Collections.Concurrent;
using System.Security.Cryptography;
using AutoFixture;
using StreamStore.Testing.Models;

namespace StreamStore.Testing
{
    public class MemoryDatabase
    {

        readonly ConcurrentDictionary<Id, StreamItem> store = new ConcurrentDictionary<Id, StreamItem>();

        public MemoryDatabase()
        {
            Fill();
        }

        public Id GetExistingStreamId()
        {
            return store.Keys.Skip(RandomStreamIndex()).First();
        }

        public StreamItem GetExistingStream()
        {
            return store[GetExistingStreamId()];
        }

        public StreamItem? Get(Id id)
        {
            store.TryGetValue(id, out var stream);
            return stream;
        }

        public async Task CopyTo(IStreamDatabase database)
        {
            foreach (var pair in store)
            {
                await database
                    .BeginAppendAsync(pair.Key)
                    .AddRangeAsync(pair.Value.Events)
                    .SaveChangesAsync();
            }

        }

        void Fill(int count = 100)
        {
            var fixture = new Fixture();
            var ids = Enumerable.Range(0, count).Select(i => Generated.Id).ToArray();
            Parallel.ForEach(ids, id => store.TryAdd(id, GenerateStream(fixture, id)));
        }

        static StreamItem GenerateStream(Fixture fixture, Id id)
        {
            return new StreamItem(id, fixture.CreateMany<EventItem>(100).ToArray());
        }

        int RandomStreamIndex()
        {
            return RandomNumberGenerator.GetInt32(0, store.Keys.Count - 1);
        }
    }
}
