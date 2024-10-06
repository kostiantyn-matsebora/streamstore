namespace StreamStore.S3.B2
{
    public class B2S3Credentials
    {
        public string AccessKeyId { get; }
        public string AccessKey { get; }

        internal B2S3Credentials(string accessKeyId, string accessKey)
        {
            AccessKeyId = accessKeyId;
            AccessKey = accessKey;
        }
    }
}
