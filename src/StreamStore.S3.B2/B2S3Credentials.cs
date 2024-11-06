using System.Security;

namespace StreamStore.S3.B2
{
    public class B2S3Credentials
    {
        public SecureString AccessKeyId { get; }
        public SecureString AccessKey { get; }

        internal B2S3Credentials(SecureString accessKeyId, SecureString accessKey)
        {
            AccessKeyId = accessKeyId;
            AccessKey = accessKey;
        }
    }
}
