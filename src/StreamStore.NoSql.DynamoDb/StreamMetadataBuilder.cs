using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using StreamStore.Extensions;
using StreamStore.Storage;

namespace StreamStore.NoSql.DynamoDb
{
    internal class StreamMetadataBuilder
    {

        Id streamId;
        Dictionary<string, AttributeValue>? attributes;

        public StreamMetadataBuilder WithAttributes(Dictionary<string, AttributeValue> attributes)
        {
            this.attributes = attributes.ThrowIfNull(nameof(attributes));

            return this;
        }

        public StreamMetadataBuilder WithStreamId(Id streamId)
        {
            this.streamId = streamId.ThrowIfHasNoValue(nameof(streamId));
            return this;
        }

        public StreamMetadata Build()
        {
            attributes.ThrowIfNull(nameof(attributes));
            streamId.ThrowIfNull(nameof(streamId));

            return new StreamMetadata(
                id: streamId, 
                revision: Convert.ToInt32(attributes![AttributeNames.Revision].N), 
                lastModified: DateTime.Parse(attributes[AttributeNames.Timestamp].S)
                );
        }
    }
}
