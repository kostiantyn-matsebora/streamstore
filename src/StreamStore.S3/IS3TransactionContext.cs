using System;
using System.Collections.Generic;
using System.Text;

namespace StreamStore.S3
{
    public interface IS3TransactionContext
    {
        public Id StreamId { get; }
        public Id TransactionId { get; }
        public string LockKey { get; }
    }
}
