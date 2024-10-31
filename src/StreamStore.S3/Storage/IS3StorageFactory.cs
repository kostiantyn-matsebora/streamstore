namespace StreamStore.S3.Storage
{
    internal interface IS3StorageFactory
    {
        public IS3Storage CreateStorage();
    }
}
