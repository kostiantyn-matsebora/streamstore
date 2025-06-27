using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
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
            var request = new DeleteItemRequest
            {
                TableName = config.TableName,
                Key = new Dictionary<string, AttributeValue>
                    {
                        { AttributeNames.StreamId,  new AttributeValue { S = streamId.ToString() } },
                    }
            };

            var response = await client.DeleteItemAsync(request);
        }

        protected override async Task<IStreamMetadata?> GetMetadataInternal(Id streamId, CancellationToken token = default)
        {
            var response = await client.QueryAsync(requests.GetMetadata(streamId));

            if (response.Count == 0) return null;

            return new StreamMetadataBuilder()
                    .WithStreamId(streamId)
                    .WithAttributes(response.Items.First())
                    .Build();
        }

        protected override async Task<IStreamEventRecord[]> ReadAsyncInternal(Id streamId, Revision startFrom, int count, CancellationToken token = default)
        {

            var response = await client.QueryAsync(requests.GetStreamEvents(streamId, startFrom, count));
            if (response.Count == 0) return Array.Empty<IStreamEventRecord>();

            return response.Items.Select(item =>
                        new StreamEventRecordBuilder()
                            .WithAttributes(item)
                            .Build())
                   .ToArray();
        }

        protected override async Task WriteAsyncInternal(Id streamId, IEnumerable<IStreamEventRecord> batch, CancellationToken token = default)
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
    }
}
