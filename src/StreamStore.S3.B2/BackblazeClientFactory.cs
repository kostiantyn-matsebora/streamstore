using System;
using System.Collections.Generic;
using System.Text;
using Bytewizer.Backblaze.Client;

namespace StreamStore.S3.B2
{
    internal class BackblazeClientFactory : IStorageClientFactory
    {
        public IStorageClient Create()
        {
            return new BackblazeClient();
        }
    }
}
