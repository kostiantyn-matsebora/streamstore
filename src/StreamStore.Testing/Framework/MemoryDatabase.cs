using System.Collections.Concurrent;
using System.Security.Cryptography;
using StreamStore.Testing.Models;

namespace StreamStore.Testing
{
    public class MemoryDatabase
    {

        readonly ConcurrentDictionary<Id, StreamRecord> store = new ConcurrentDictionary<Id, StreamRecord>();

        public MemoryDatabase(IEnumerable<Id> ids)
        {
            Fill(ids);
        }

        public MemoryDatabase(): this(GenerateIds(100))
        {
        }

        public Id RandomId => store.Keys.Skip(RandomStreamIndex()).First();

        public StreamRecord RandomStream =>  store[RandomId];
     

        public StreamRecord PeekStream()
        {
            lock (store)
            {
                store.Remove(RandomId, out var stream);
                return stream!;
            }
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

        static StreamRecord GenerateStream(Id id)
        {
            return new StreamRecord(id, Generated.EventRecords(100));
        }

        int RandomStreamIndex()
        {
            return RandomNumberGenerator.GetInt32(0, store.Keys.Count - 1);
        }
    }
}
