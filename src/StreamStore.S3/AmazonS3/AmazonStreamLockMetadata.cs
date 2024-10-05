using System;
using System.Collections.Generic;


namespace StreamStore.S3.AmazonS3
{
  

    internal class AmazonStreamLockMetadata
    {
        const string LockAcquiredAt = "x-amz-meta-stream-lock-acquired-at";
        Dictionary<string, string> metadata = new Dictionary<string, string>();

        public string this[string key] => metadata[key];

        public ICollection<string> Keys => metadata.Keys;

        public AmazonStreamLockMetadata()
        {
            metadata.Add(LockAcquiredAt, DateTime.UtcNow.ToString());
        }
    }
}
