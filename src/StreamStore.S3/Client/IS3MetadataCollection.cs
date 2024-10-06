namespace StreamStore.S3.Client
{
    public interface IS3MetadataCollection : IS3ReadonlyMetadataCollection
    {
        public IS3MetadataCollection Add(string key, string value);
    }
}
