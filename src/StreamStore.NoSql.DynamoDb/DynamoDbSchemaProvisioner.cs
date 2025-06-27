using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using StreamStore.Extensions;
using StreamStore.Provisioning;

namespace StreamStore.NoSql.DynamoDb
{
    class DynamoDbSchemaProvisioner : ISchemaProvisioner
    {
        readonly IAmazonDynamoDB client;
        readonly DynamoDbConfiguration configuration;

        public DynamoDbSchemaProvisioner(IAmazonDynamoDB client, DynamoDbConfiguration configuration)
        {
            this.client = client.ThrowIfNull(nameof(client));
            this.configuration = configuration.ThrowIfNull(nameof(configuration));
        }

        public async Task ProvisionSchemaAsync(CancellationToken token)
        {
            var response = await client.CreateTableAsync(new CreateTableRequest
            {
                TableName = configuration.TableName,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition
                    {
                        AttributeName = AttributeNames.StreamId,
                        AttributeType = ScalarAttributeType.S,
                    },
                    new AttributeDefinition
                    {
                        AttributeName = AttributeNames.Revision,
                        AttributeType = ScalarAttributeType.N,
                    }
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement
                    {
                        AttributeName = AttributeNames.StreamId,
                        KeyType = KeyType.HASH,
                    },
                    new KeySchemaElement
                    {
                        AttributeName = AttributeNames.Revision,
                        KeyType = KeyType.HASH,
                    },
                },
                BillingMode = configuration.BillingMode,
            });
        }
    }
}
