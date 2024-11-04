using AutoFixture;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Integration
{
    public class S3IntegrationFixture : IDisposable
    {

        readonly object lockObject = new object();
        bool alreadyCopied = false;
        bool disposedValue;
        IStreamDatabase? database;

        public MemoryDatabase Container { get; }

        public S3IntegrationFixture()
        {
            Container = new MemoryDatabase(new MemoryDatabaseOptions { Capacity = 10, EventPerStream = 10 });
        }

        public void CopyTo(IStreamDatabase database)
        {
            if (alreadyCopied)
            {
                return;
            }

            lock (lockObject)
            {
                if (alreadyCopied)
                {
                    return;
                }
                this.database = database;
                Container.CopyTo(database);
                alreadyCopied = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && database != null)
                {
                    foreach (var stream in Container)
                    {
                        database!.DeleteAsync(stream.Id).Wait();
                    }
                }
                disposedValue = true;
            }
        }
        
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
