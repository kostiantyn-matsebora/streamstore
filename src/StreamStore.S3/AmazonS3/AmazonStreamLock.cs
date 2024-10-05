﻿using Amazon.S3.Model;
using Amazon.S3;
using System.Threading.Tasks;
using System.Threading;
using System;
using StreamStore.S3.Lock;


namespace StreamStore.S3.AmazonS3
{
    internal class AmazonStreamLock: IS3StreamLock
    {
        readonly Id streamId;
        string? lockBucketName;
        AmazonS3Client? client;

        public AmazonStreamLock(Id streamId, string lockBucketName, AmazonS3Client client) 
        {
            if (streamId == Id.None)
                throw new ArgumentException("Stream Id must be specified", nameof(streamId));
            this.streamId = streamId;

            if (string.IsNullOrWhiteSpace(lockBucketName))
                throw new ArgumentException("Bucket name must be specified", nameof(lockBucketName));
            this.lockBucketName = lockBucketName;
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<bool> AcquireAsync(CancellationToken token)
        {
            var request = new PutObjectRequest
            {
                BucketName = lockBucketName,
                Key = streamId,
                ContentBody = Guid.NewGuid().ToString(),
                ObjectLockLegalHoldStatus = ObjectLockLegalHoldStatus.On
            };

            var response = await client!.PutObjectAsync(request, token);

            return response.HttpStatusCode != System.Net.HttpStatusCode.OK;
        }

        public async Task ReleaseAsync(CancellationToken token)
        {
            var request = new PutObjectLegalHoldRequest
            {
                BucketName = $"{lockBucketName}_locks",
                Key = streamId,
                LegalHold = new ObjectLockLegalHold
                {
                    Status = ObjectLockLegalHoldStatus.Off
                },


            };
            await client!.PutObjectLegalHoldAsync(request, token);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                client = null;
                lockBucketName = null;
            }
        }
    }
}
