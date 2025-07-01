using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using StreamStore.Exceptions.Appending;
using StreamStore.Extensions;
using StreamStore.Storage;

namespace StreamStore.NoSql.DynamoDb
{
    internal class DynamoDbStreamStorage : StreamStorageBase
    {
        readonly DynamoDbConfiguration config;
        readonly IAmazonDynamoDB client;
        readonly DynamoDbRequests requests;

        public DynamoDbStreamStorage(DynamoDbConfiguration config, IAmazonDynamoDB client, DynamoDbRequests requests)
        {
            this.config = config.ThrowIfNull(nameof(config));
            this.client = client.ThrowIfNull(nameof(client));
            this.requests = requests.ThrowIfNull(nameof(requests));
        }

        protected override async Task DeleteAsyncInternal(Id streamId, CancellationToken token = default)
        {
            var metadata = await GetMetadataInternal(streamId, token);
            if (metadata == null) return;

            foreach (var revisions in CreateRevisionBatchEnumerable(metadata))
            {
                var request = requests.DeleteStreamRevisions(streamId, revisions);
                await client.BatchWriteItemAsync(request, token);
            }
        }


        protected override async Task<IStreamMetadata?> GetMetadataInternal(Id streamId, CancellationToken token = default)
        {
            var request = requests.GetMetadata(streamId);
            var response = await client.QueryAsync(request);

            if (response.Count == 0) return null;

            return new StreamMetadataBuilder()
                    .WithStreamId(streamId)
                    .WithAttributes(response.Items.First())
                    .Build();
        }

        protected override async Task<IStreamEventRecord[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {
            List<IStreamEventRecord> records = new List<IStreamEventRecord>();

            await foreach (var batch in CreateStreamEventBatchAsyncEnumerable(streamId, startFrom, count, token))
            {
                records.AddRange(batch);
            }

            return records.ToArray();
        }

        protected override async Task WriteAsyncInternal(Id streamId, IEnumerable<IStreamEventRecord> batch, CancellationToken token = default)
        {
            if (batch.Count() > DynamoDbConfiguration.WritingBatchSize)
                throw new InvalidOperationException($"Writing revisions size exceeds  {DynamoDbConfiguration.WritingBatchSize} items limit.");

            try
            {
                var items =
                    batch.Select(r =>
                        new TransactWriteItem
                        {
                            Put = new PutItemBuilder()
                                    .WithTableName(config.TableName)
                                    .WithStreamId(streamId)
                                    .WithRecord(r)
                                    .Build()
                        }
                        ).ToList();

                var result = await client.TransactWriteItemsAsync(new TransactWriteItemsRequest() { TransactItems = items });
            }
            catch (TransactionCanceledException ex)
            {
                if (ex.CancellationReasons.Any(r => r.Code == "ConditionalCheckFailed"))
                    throw new RevisionAlreadyExistsException(streamId);
            }
        }

        async Task<IStreamEventRecord[]> ReadStreamEventBatch(Id streamId, Revision startFrom, int count, CancellationToken token)
        {
            var response = await client.QueryAsync(requests.GetStreamEvents(streamId, startFrom, count));

            return response.Items.Select(item =>
                    new StreamEventRecordBuilder()
                        .WithAttributes(item)
                        .Build())
                        .ToArray();
        }

        IAsyncEnumerable<IStreamEventRecord[]> CreateStreamEventBatchAsyncEnumerable(Id streamId, Revision startFrom, int count, CancellationToken token)
        {
            return new StreamEventMetadataBatchEnumerable<IStreamEventRecord>(
                parameters: new StreamEventReadingParameters
                            {
                                Count = count,
                                StartFrom = startFrom,
                                BatchSize = config.ReadingBatchSize
                            },
                 reader: (startFrom, count) => ReadStreamEventBatch(streamId, startFrom, count, token)
                );
        }

        static PagingEnumerable<int> CreateRevisionBatchEnumerable(IStreamMetadata metadata)
        {
            return new PagingEnumerable<int>(Enumerable.Range(Revision.One, metadata.Revision), DynamoDbConfiguration.DeletingBatchSize);
        }

    }
}
