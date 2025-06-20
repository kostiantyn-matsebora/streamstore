﻿using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Testing.Models;

namespace StreamStore.Testing
{

    public class MemoryStorageOptions
    {
        public int Capacity { get; set; } = 100;

        public int EventPerStream { get; set; } = 100;
    }
    public class MemoryStorage: IEnumerable<TestStreamRecord>
    {

        readonly ConcurrentDictionary<Id, TestStreamRecord> store = new ConcurrentDictionary<Id, TestStreamRecord>();

        public MemoryStorage() : this(new MemoryStorageOptions())
        {
        }

        public MemoryStorage(MemoryStorageOptions options)
        {
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

        public void CopyTo(IStreamStorage storage)
        {
            // Copying events from source
            var tasks = store.Select(async pair =>
            {
                await storage.WriteAsync(pair.Key, pair.Value.Events, CancellationToken.None);
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

        static TestStreamRecord GenerateStream(Id id, int eventPerStream)
        {
            return new TestStreamRecord(id, Generated.StreamEventRecords.Many(eventPerStream));
        }

        int RandomStreamIndex()
        {
            return RandomNumberGenerator.GetInt32(0, store.Keys.Count - 1);
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
