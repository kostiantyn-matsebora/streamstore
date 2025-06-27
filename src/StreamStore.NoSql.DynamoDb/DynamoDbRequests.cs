using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.Model;
using StreamStore.Extensions;

namespace StreamStore.NoSql.DynamoDb
{
    internal class DynamoDbRequests
    {
        readonly DynamoDbConfiguration config;

        public DynamoDbRequests(DynamoDbConfiguration config)
        {
            this.config = config.ThrowIfNull(nameof(config));
        }
        public QueryRequest GetMetadata(Id streamId)
        {
            return new QueryRequest
            {
                TableName = config.TableName,
                ScanIndexForward = false,
                ConsistentRead = true,
                Limit = 1,
                AttributesToGet = new string[] { AttributeNames.Revision, AttributeNames.Timestamp }.ToList(),
                KeyConditionExpression = $"{AttributeNames.StreamId} = :streamId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":streamId", new AttributeValue { S = streamId.ToString() } },
                }
            };
        }

        public QueryRequest GetStreamEvents(Id streamId, int startFrom, int count)
        {

            return new QueryRequest
            {
                TableName = config.TableName,
                ScanIndexForward = true,
                ConsistentRead = true,
                Limit = count,
                KeyConditionExpression = $"{AttributeNames.StreamId} = :streamId AND {AttributeNames.Revision} >= :startFrom",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":streamId", new AttributeValue { S = streamId.ToString() } },
                    { ":startFrom", new AttributeValue { N = startFrom.ToString() } },
                }
            };
        }
    }
}
