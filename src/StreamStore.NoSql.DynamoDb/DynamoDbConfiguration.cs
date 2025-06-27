using Amazon.DynamoDBv2;

namespace StreamStore.NoSql.DynamoDb
{
    internal class DynamoDbConfiguration
    {
        public string TableName { get; set; } = "Events";
        public BillingMode BillingMode { get; set; } = BillingMode.PAY_PER_REQUEST;

        public static DynamoDbConfiguration Default => new DynamoDbConfiguration();
    }
}
