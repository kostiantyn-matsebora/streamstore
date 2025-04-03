using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using StreamStore.Testing.Models;

namespace StreamStore.Testing
{

    public class MemoryDatabaseOptions
    {
        public int Capacity { get; set; } = 100;

        public int EventPerStream { get; set; } = 100;
    }
    public class MemoryDatabase: IEnumerable<StreamRecord>
    {

        readonly ConcurrentDictionary<Id, StreamRecord> store = new ConcurrentDictionary<Id, StreamRecord>();

        public MemoryDatabase() : this(new MemoryDatabaseOptions())
        {
        }

        public MemoryDatabase(MemoryDatabaseOptions options)
        {
            Fill(GenerateIds(options.Capacity), options.EventPerStream);
        }

        public Id RandomId => store.Keys.Skip(RandomStreamIndex()).First();

        public StreamRecord RandomStream => store[RandomId];


        public StreamRecord PeekStream()
        {
            lock (store)
            {
                store.Remove(store.First().Key, out var stream);
                return stream!;
            }
        }

        public void CopyTo(IStreamStorage database)
        {
            // Copying events from source
            var tasks = store.Select(async pair =>
            {
                await database
                 .BeginAppendAsync(pair.Key)
                 .AddRangeAsync(pair.Value.Events)
                 .SaveChangesAsync();
            });

            Task.WaitAll(tasks.ToArray());
        }

        void Fill(IEnumerable<Id> ids, int eventPerStream)
        {
            Parallel.ForEach(ids, id => store.TryAdd(id, GenerateStream(id, eventPerStream)));
        }

        static Id[] GenerateIds(int capacity)
        {
            return Enumerable.Range(0, capacity).Select(i => Generated.Primitives.Id).ToArray();
        }

        static StreamRecord GenerateStream(Id id, int eventPerStream)
        {
            return new StreamRecord(id, Generated.Many(eventPerStream));
        }

        int RandomStreamIndex()
        {
            return RandomNumberGenerator.GetInt32(0, store.Keys.Count - 1);
        }

        public IEnumerator<StreamRecord> GetEnumerator()
        {
            return store.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return store.Values.GetEnumerator();
        }
    }
}
