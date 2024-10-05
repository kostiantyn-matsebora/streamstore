using System;
using System.Collections.Generic;
using Amazon.S3.Model;
using StreamStore.S3.Client;

namespace StreamStore.S3.AmazonS3
{
   

    internal sealed class AmazonMetadataCollection: IS3ReadonlyMetadataCollection
    {
        readonly MetadataCollection collection;

        public string this[string key] => collection[key];

        public ICollection<string> Keys => collection.Keys;

        public AmazonMetadataCollection(MetadataCollection metadata)
        {
            this.collection = metadata ?? throw new ArgumentNullException(nameof(metadata));
        }
    }
}
