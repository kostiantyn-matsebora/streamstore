using System;
using System.Collections.Concurrent;
using B2Net;
using B2Net.Models;
using StreamStore.S3.Client;

namespace StreamStore.S3.B2
{
    internal class B2S3Factory : IS3Factory
    {
        readonly B2StreamDatabaseSettings settings;
        ConcurrentBag<Id> streamLocks = new ConcurrentBag<Id>();

        B2Client? client;

        public B2S3Factory(B2StreamDatabaseSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public IS3Client CreateClient()
        {
            return new B2S3Client();
        }

        public IS3StreamLock CreateLock(Id streamId)
        {
            return new B2S3StreamLock(streamId, settings.BucketId!, settings.BucketName!, CreateB2Client());

        }

        B2Client CreateB2Client()
        {
            var options = new B2Options()
            {
                KeyId = settings.Credentials!.AccessKeyId,
                ApplicationKey = settings.Credentials!.AccessKey,
                BucketId = settings.BucketId,
            };

            return new B2Client(options, authorizeOnInitialize: true);
        }
    }
}
