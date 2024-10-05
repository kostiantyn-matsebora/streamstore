using System;

using System.Threading;
using System.Threading.Tasks;


namespace StreamStore.S3
{
    public sealed class S3StreamDatabase : IStreamDatabase
    {
        readonly S3AbstractFactory factory;
        internal S3StreamDatabase(S3AbstractFactory factory)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public IStreamUnitOfWork BeginAppend(string streamId, int expectedStreamVersion = 0)
        {
            return new S3StreamUnitOfWork(streamId, expectedStreamVersion, factory);
        }

        public async Task DeleteAsync(string streamId, CancellationToken cancellationToken)
        {
            using var transaction = await S3StreamTransaction.BeginAsync(streamId, factory);
            {
                try
                {
                    using var deleter = factory.CreateDeleter(streamId);
                    await deleter.DeleteAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<StreamRecord?> FindAsync(string streamId, CancellationToken cancellationToken)
        {
            using var loader = factory.CreateLoader(streamId);
            var stream = await loader.LoadAsync(cancellationToken);
            if (stream == null) return null;
            return new StreamRecord(streamId, stream.Events);
        }

        public async Task<StreamMetadataRecord?> FindMetadataAsync(string streamId, CancellationToken cancellationToken)
        {
          using var loader = factory.CreateLoader(streamId); 
          var stream = await loader.LoadAsync(cancellationToken);
          if (stream == null) return null;
          return new StreamMetadataRecord(streamId, stream.Events);
        }
    }
}