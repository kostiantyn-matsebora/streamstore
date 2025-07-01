using Amazon.DynamoDBv2;

namespace StreamStore.NoSql.DynamoDb
{
    public class DynamoDbConfiguration
    {
        // Limitation of DynamoDB: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/transaction-apis.html#transaction-apis-txwriteitems
        public const int WritingBatchSize = 100;
        // Limitation of DynamoDB: https://docs.aws.amazon.com/amazondynamodb/latest/APIReference/API_BatchWriteItem.html
        public const int DeletingBatchSize = 25;

        public string TableName { get; set; } = "Events";
        public BillingMode BillingMode { get; set; } = BillingMode.PAY_PER_REQUEST;
        public int ReadingBatchSize { get; set; } = 100;
        public static DynamoDbConfiguration Default => new DynamoDbConfiguration();
    }
}
