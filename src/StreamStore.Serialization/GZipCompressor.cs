using System.IO;
using System.IO.Compression;


namespace StreamStore.Serialization
{
    public static class GZipCompressor
    {

        public static byte[] Compress(byte[] serialized)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    gzip.Write(serialized, 0, serialized.Length);
                }
                return ms.ToArray();
            }
        }

        public static byte[] Decompress(byte[] compressed)
        {
            using (MemoryStream ms = new MemoryStream(compressed))
            using (GZipStream gzip = new GZipStream(ms, CompressionMode.Decompress))
            using (MemoryStream outBuffer = new MemoryStream())
            {
                gzip.CopyTo(outBuffer);
                return outBuffer.ToArray();
            }
        }
    }
}
