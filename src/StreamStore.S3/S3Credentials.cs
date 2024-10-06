namespace StreamStore.S3
{
    public class S3Credentials
    {
        public string AccessKeyId { get; }
        public string AccessKey { get;  }

        internal S3Credentials(string accessKeyId, string accessKey)
        {
            AccessKeyId = accessKeyId;
            AccessKey = accessKey;
        }
    }
}
