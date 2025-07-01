using Amazon.DynamoDBv2;
using Microsoft.Extensions.Configuration;

namespace StreamStore.NoSql.DynamoDb
{
    public interface IDynamoDbConfigurator
    {
        IDynamoDbConfigurator UseConfiguration(IConfiguration configuration, string configSection = "AWS");
        IDynamoDbConfigurator WithTableName(string tableName);
        IDynamoDbConfigurator WithBillingMode(BillingMode mode);
        IDynamoDbConfigurator WithReadingBatchSize(int batchSize);
    }
}
