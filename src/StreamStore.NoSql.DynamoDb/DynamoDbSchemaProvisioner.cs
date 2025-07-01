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
        const int delayBetweenAttempts = 1000;
        const int attemptCount = 100;

        public DynamoDbSchemaProvisioner(IAmazonDynamoDB client, DynamoDbConfiguration configuration)
        {
            this.client = client.ThrowIfNull(nameof(client));
            this.configuration = configuration.ThrowIfNull(nameof(configuration));
        }

        public async Task ProvisionSchemaAsync(CancellationToken token)
        {
            try
            {

                var response = await client.CreateTableAsync(new CreateTableRequest
                {
                    TableName = configuration.TableName,
                    BillingMode = configuration.BillingMode,
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
                        KeyType = KeyType.RANGE,
                    },
                },

                });

                await WaitForCreationAsync(response.TableDescription.TableStatus, token);
            }
            catch (ResourceInUseException ex)
            {

                if (
                       ex.Message.Contains("Table already exists")
                    || ex.Message.Contains("Table is being created")) return;
                throw;
            }
        }

        async Task WaitForCreationAsync(TableStatus tableStatus, CancellationToken token)
        {
            int count = attemptCount;
            var status = tableStatus;

            while (status != TableStatus.ACTIVE && count > 0)
            {
                token.ThrowIfCancellationRequested();
                count--;

                await Task.Delay(delayBetweenAttempts);
                status = await GetTableStatusAsync(token);
            }

            if (status != TableStatus.ACTIVE) 
                throw new TableNotFoundException(configuration.TableName);
        }

        async Task<TableStatus> GetTableStatusAsync(CancellationToken token)
        {
            var result = await client.DescribeTableAsync(configuration.TableName, token);
            return result.Table.TableStatus;
        }
    }
}
