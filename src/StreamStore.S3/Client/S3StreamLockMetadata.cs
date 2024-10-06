using System;
using System.Collections.Generic;


namespace StreamStore.S3.Client
{
    internal class S3StreamLockMetadata
    {
        const string LockAcquiredAt = "x-amz-meta-stream-lock-acquired-at";
        readonly Dictionary<string, string> metadata = new Dictionary<string, string>();

        public string this[string key] => metadata[key];

        public ICollection<string> Keys => metadata.Keys;

        public S3StreamLockMetadata()
        {
            metadata.Add(LockAcquiredAt, DateTime.UtcNow.ToString());
        }
    }
}
