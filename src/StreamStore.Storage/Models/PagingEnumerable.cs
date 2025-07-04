using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StreamStore.Extensions;

namespace StreamStore.Storage
{
    public class PagingEnumerable<T> : IEnumerable<T[]>
    {
        readonly int pageSize;
        readonly IEnumerable<T> source;

        public PagingEnumerable(IEnumerable<T> source, int pageSize)
        {
            this.pageSize = pageSize;
            this.source = source.ThrowIfNull(nameof(source));
        }

        public IEnumerator<T[]> GetEnumerator()
        {
            return new PagingEnumerator<T>(source, pageSize);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class PagingEnumerator<T> : IEnumerator<T[]>
    {
        readonly int pageSize;
        IEnumerable<T> source;
        int currentPage = 0;
        bool disposedValue;

        public PagingEnumerator(IEnumerable<T> source, int pageSize)
        {
            this.pageSize = pageSize;
            this.source = source;
        }

        public T[] Current => GetCurrent();

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public bool MoveNext()
        {
            if (source.Count() <= currentPage * pageSize) return false;
            currentPage++;
            return true;
        }

        public void Reset()
        {
            currentPage = 0;
        }

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    source = null!;
                }
                disposedValue = true;
            }
        }

        T[] GetCurrent()
        {
            var count = pageSize;
            
            if (source.Count() < currentPage * pageSize)
            {
                count = source.Count() - (currentPage - 1) * pageSize;
            }

            return source.Skip((currentPage - 1) * pageSize).Take(count).ToArray();
        }
    }
}
