using System.Collections.Concurrent;
using System.Security.Cryptography;
using AutoFixture;
using StreamStore.Testing.Models;

namespace StreamStore.Testing
{
    public class MemoryDatabase
    {

        readonly ConcurrentDictionary<Id, StreamItem> store = new ConcurrentDictionary<Id, StreamItem>();

        public MemoryDatabase(IEnumerable<Id> ids)
        {
            Fill(ids);
        }

        public MemoryDatabase(): this(GenerateIds(100))
        {
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

        void Fill(IEnumerable<Id> ids)
        {
            Parallel.ForEach(ids, id => store.TryAdd(id, GenerateStream(id)));
        }

        private static Id[] GenerateIds(int count)
        {
            return Enumerable.Range(0, count).Select(i => Generated.Id).ToArray();
        }

        static StreamItem GenerateStream(Id id)
        {
            return new StreamItem(id, Generated.EventItems(100));
        }

        int RandomStreamIndex()
        {
            return RandomNumberGenerator.GetInt32(0, store.Keys.Count - 1);
        }
    }
}
