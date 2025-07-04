using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Amazon.DynamoDBv2.Model;
using StreamStore.Extensions;

namespace StreamStore.NoSql.DynamoDb
{
    internal class PutItemBuilder
    {

        IStreamEventRecord? record;
        string? tableName;
        Id streamId;

        public PutItemBuilder WithRecord(IStreamEventRecord record)
        {
            this.record = record.ThrowIfNull(nameof(record));
            return this;
        }

        public PutItemBuilder WithTableName(string tableName)
        {
            this.tableName = tableName;
            return this;
        }

        public PutItemBuilder WithStreamId(Id streamId)
        {
            this.streamId = streamId;
            return this;
        }

        public Put Build()
        {
            streamId.ThrowIfHasNoValue(nameof(streamId));
            record.ThrowIfNull(nameof(record));
            tableName.ThrowIfNull(nameof(tableName));

            var attributes = new Dictionary<string, AttributeValue>
                {
                    { AttributeNames.StreamId, new AttributeValue() { S = streamId.ToString() } },
                    { AttributeNames.Revision, new  AttributeValue() { N = record!.Revision.ToString() } },
                    { AttributeNames.Id, new  AttributeValue() { S = record.Id.ToString() } },
                    { AttributeNames.Timestamp, new AttributeValue { S = record.Timestamp.ToUniversalTime().ToString(CultureInfo.InvariantCulture) } },
                    { AttributeNames.Data, new AttributeValue { B = new MemoryStream(record.Data) } },
                };

            if (record.CustomProperties.NotNullAndNotEmpty())
            {
                foreach (var property in record.CustomProperties!)
                {
                    var (key, value) = CreatePropertyAttribute(property);
                    attributes.Add(key, value);
                }
            }

            return new Put()
            {
                TableName = tableName,
                Item = attributes,
                ConditionExpression = "attribute_not_exists(Revision)"
            };
        }

        private static (string, AttributeValue) CreatePropertyAttribute(KeyValuePair<string, string> property)
        {
            var attributeValue = property.Value != null ? new AttributeValue() { S = property.Value } : new AttributeValue() { NULL = true };
            return (AttributeNames.CustomProperty(property.Key), attributeValue);
        }
    }
}
