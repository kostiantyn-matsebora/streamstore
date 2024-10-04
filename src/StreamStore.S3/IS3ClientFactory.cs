using System;
using Amazon.S3;

namespace StreamStore.S3
{
    public  interface IS3ClientFactory
    {
        IS3Client CreateClient();
    }

    
}
