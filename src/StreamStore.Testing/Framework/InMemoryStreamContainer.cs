using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Storage;
using StreamStore.Testing.Models;

namespace StreamStore.Testing
{

    public class InMemoryStreamContainerOptions
    {
        public int Capacity { get; set; } = 100;

        public int EventPerStream { get; set; } = 100;

        public int CopyDegreeOfParallelism { get; set; } = 1;

        public int WriteBatchSize { get; set; } = 100;
    }

    public class InMemoryStreamContainer : IEnumerable<TestStreamRecord>
    {

        readonly ConcurrentDictionary<Id, TestStreamRecord> store = new ConcurrentDictionary<Id, TestStreamRecord>();
        readonly InMemoryStreamContainerOptions options;

        public InMemoryStreamContainer() : this(new InMemoryStreamContainerOptions())
        {
        }

        public InMemoryStreamContainer(InMemoryStreamContainerOptions options)
        {
            this.options = options;
            Fill(GenerateIds(options.Capacity), options.EventPerStream);
        }

        public Id RandomId => store.Keys.Skip(RandomStreamIndex()).First();

        public TestStreamRecord RandomStream => store[RandomId];


        public TestStreamRecord PeekStream()
        {
            lock (store)
            {
                store.Remove(store.First().Key, out var stream);
                return stream!;
            }
        }

        public async Task CopyToAsync(IStreamStorage storage)
        {
            await Parallel.ForEachAsync(
                store,
                new ParallelOptions { MaxDegreeOfParallelism = options.CopyDegreeOfParallelism },
                async (stream, token) =>
                {
                    foreach (var batch in PagingEnumerable(stream.Value.Events))
                    {
                        await storage.WriteAsync(stream.Key, batch, CancellationToken.None);
                    }
                });
        }

        void Fill(IEnumerable<Id> ids, int eventPerStream)
        {
            Parallel.ForEach(ids, id => store.TryAdd(id, GenerateStream(id, eventPerStream)));
        }

        static Id[] GenerateIds(int capacity)
        {
            return Enumerable.Range(0, capacity).Select(i => Generated.Primitives.Id).ToArray();
        }

        static TestStreamRecord GenerateStream(Id id, int eventPerStream)
        {
            return new TestStreamRecord(id, Generated.StreamEventRecords.Many(eventPerStream));
        }

        int RandomStreamIndex()
        {
            return RandomNumberGenerator.GetInt32(0, store.Keys.Count - 1);
        }

        PagingEnumerable<IStreamEventRecord> PagingEnumerable(RevisionedItemCollection<IStreamEventRecord> events)
        {
            return new PagingEnumerable<IStreamEventRecord>(events.ToArray(), options.WriteBatchSize);
        }

        public IEnumerator<TestStreamRecord> GetEnumerator()
        {
            return store.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return store.Values.GetEnumerator();
        }
    }
}
