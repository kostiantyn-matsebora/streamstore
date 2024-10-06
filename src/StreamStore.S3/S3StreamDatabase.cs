using System;

using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Operations;


namespace StreamStore.S3
{
    public sealed class S3StreamDatabase : IStreamDatabase
    {
        readonly IS3Factory factory;
        internal S3StreamDatabase(IS3Factory factory)
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
            await TryDeleteAsync(streamId, transaction, cancellationToken);
        }

        private async Task TryDeleteAsync(string streamId, S3StreamTransaction transaction, CancellationToken cancellationToken)
        {
            try
            {
                using var deleter = S3StreamDeleter.New(streamId, factory.CreateClient());
                await deleter.DeleteAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<StreamRecord?> FindAsync(string streamId, CancellationToken cancellationToken)
        {
            using var loader = new S3StreamLoader(streamId, factory.CreateClient());
            var stream = await loader.LoadAsync(cancellationToken);
            if (stream == null) return null;
            return new StreamRecord(streamId, stream.Events);
        }

        public async Task<StreamMetadataRecord?> FindMetadataAsync(string streamId, CancellationToken cancellationToken)
        {
          using var loader = new S3StreamLoader(streamId, factory.CreateClient());
          var stream = await loader.LoadAsync(cancellationToken);
          if (stream == null) return null;
          return new StreamMetadataRecord(streamId, stream.Events);
        }
    }
}