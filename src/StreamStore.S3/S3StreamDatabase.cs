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

        public Task<IStreamUnitOfWork> BeginAppendAsync(Id streamId, Revision expectedStreamVersion, CancellationToken token = default)
        {
            return Task.FromResult((IStreamUnitOfWork)new S3StreamUnitOfWork(streamId, expectedStreamVersion, factory));
        }

        public async Task DeleteAsync(Id streamId, CancellationToken cancellationToken)
        {
            using var transaction = 
                await S3StreamTransaction
                .BeginAsync(S3TransactionContext.New(streamId), factory);

            await TryDeleteAsync(streamId, transaction, cancellationToken);
        }

     

        public async Task<StreamRecord?> FindAsync(Id streamId, CancellationToken token = default)
        {
            S3Stream? stream;
            await using (var client = factory.CreateClient())
            stream = await S3StreamLoader
                    .New(S3StreamContext.Persistent(streamId), client)
                    .LoadAsync(token);

            if (stream == null) return null;

            return new StreamRecord(streamId, stream.Events);
        }

        public async Task<StreamMetadataRecord?> FindMetadataAsync(Id streamId, CancellationToken token = default)
        {
            S3StreamMetadata? metadata;
            await using (var client = factory.CreateClient())
                metadata = await S3StreamMetadataLoader
                        .New(S3StreamContext.Persistent(streamId), client)
                        .LoadAsync(token);

            if (metadata == null) return null;

            return  metadata.ToRecord();
        }

        async Task TryDeleteAsync(Id streamId, S3StreamTransaction transaction, CancellationToken token)
        {
            try
            {
                await using (var client = factory.CreateClient())
                    await
                        S3StreamDeleter
                        .New(S3StreamContext.Persistent(streamId), client)
                        .DeleteAsync(token);

                await transaction.CommitAsync(token);

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}