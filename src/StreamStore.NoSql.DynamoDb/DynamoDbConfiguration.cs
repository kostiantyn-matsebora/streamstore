using Amazon.DynamoDBv2;

namespace StreamStore.NoSql.DynamoDb
{
    public class DynamoDbConfiguration
    {
        public const int WritingBatchSize = 100;

        public string TableName { get; set; } = "Events";
        public BillingMode BillingMode { get; set; } = BillingMode.PAY_PER_REQUEST;

        public int ReadingBatchSize { get; set; } = 100;
        public int DeletingBatchSize { get; set; } = 25;

        public static DynamoDbConfiguration Default => new DynamoDbConfiguration();
    }
}
