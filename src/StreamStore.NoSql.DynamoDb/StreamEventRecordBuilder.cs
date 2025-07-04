using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Amazon.DynamoDBv2.Model;
using StreamStore.Extensions;

namespace StreamStore.NoSql.DynamoDb
{
    internal class StreamEventRecordBuilder
    {
        Dictionary<string, AttributeValue>? attributes;
        public StreamEventRecordBuilder WithAttributes(Dictionary<string, AttributeValue> attributes)
        {
            this.attributes = attributes.ThrowIfNull(nameof(attributes));
            return this;
        }

        public StreamEventRecord Build()
        {
            attributes.ThrowIfNull(nameof(attributes));
            return new StreamEventRecord
            {
                Id = attributes![AttributeNames.Id].S,
                Data = attributes![AttributeNames.Data].B.ToArray(),
                Timestamp = DateTime.Parse(attributes![AttributeNames.Timestamp].S, CultureInfo.InvariantCulture),
                Revision = Convert.ToInt32(attributes![AttributeNames.Revision].N),
                CustomProperties = ExtractCustomProperties()
            };
        }

        private IReadOnlyDictionary<string, string> ExtractCustomProperties()
        {
            var customPropertyKeys = attributes!.Keys.Where(k => k.StartsWith(AttributeNames.CustomPropertyPrefix)).ToArray();

            if (!customPropertyKeys.Any()) return new Dictionary<string, string>();

            return new Dictionary<string, string>(customPropertyKeys.Select(ExtractCustomProperty));
        }

        KeyValuePair<string, string> ExtractCustomProperty(string key)
        {
            return new KeyValuePair<string, string>(ExtractCustomPropertyKey(key), attributes![key].S);
        }

        static string ExtractCustomPropertyKey(string key)
        {
            return key.Substring(AttributeNames.CustomPropertyPrefix.Length);
        }
    }
}
