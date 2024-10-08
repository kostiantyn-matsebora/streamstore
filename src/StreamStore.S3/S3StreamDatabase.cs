using System;

using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;
using StreamStore.S3.Models;
using StreamStore.S3.Operations;


namespace StreamStore.S3
{
    public sealed class S3StreamDatabase : IStreamDatabase
    {
        readonly IS3Factory factory;
        public S3StreamDatabase(IS3Factory factory)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public IStreamUnitOfWork BeginAppend(string streamId, int expectedStreamVersion = 0)
        {
            return new S3StreamUnitOfWork(streamId, expectedStreamVersion, factory);
        }

        public async Task DeleteAsync(string streamId, CancellationToken cancellationToken)
        {
            using var transaction = 
                await S3StreamTransaction
                .BeginAsync(S3TransactionContext.New(streamId), factory);

            await TryDeleteAsync(streamId, transaction, cancellationToken);
        }

     

        public async Task<StreamRecord?> FindAsync(string streamId, CancellationToken cancellationToken)
        {
            S3Stream? stream;
            await using (var client = factory.CreateClient())
            stream = await S3StreamLoader
                    .New(S3StreamContext.Persistent(streamId), client)
                    .LoadAsync(cancellationToken);

            if (stream == null) return null;

            return new StreamRecord(streamId, stream.Events);
        }

        public async Task<StreamMetadataRecord?> FindMetadataAsync(string streamId, CancellationToken cancellationToken)
        {
            S3StreamMetadata? metadata;
            await using (var client = factory.CreateClient())
                metadata = await S3StreamMetadataLoader
                        .New(S3StreamContext.Persistent(streamId), client)
                        .LoadAsync(cancellationToken);

            if (metadata == null) return null;

            return  metadata.ToRecord();
        }

        async Task TryDeleteAsync(string streamId, S3StreamTransaction transaction, CancellationToken cancellationToken)
        {
            try
            {
                await using (var client = factory.CreateClient())
                    await
                        S3StreamDeleter
                        .New(S3StreamContext.Persistent(streamId), client)
                        .DeleteAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}