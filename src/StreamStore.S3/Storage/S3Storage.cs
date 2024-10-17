using StreamStore.S3.Client;


namespace StreamStore.S3.Storage
{
    internal class S3Storage
    {
        public S3StreamStorage Persistent { get; }
        public S3StreamStorage Transient { get; }
        public S3ObjectStorage Locks { get; }

        public S3Storage(IS3ClientFactory factory)
        {
            Persistent = new S3StreamStorage(S3ContainerPath.Root.Combine("persistent-streams"), factory);
            Transient = new S3StreamStorage(S3ContainerPath.Root.Combine("transient-streams"), factory);
            Locks = new S3ObjectStorage(S3ContainerPath.Root.Combine("locks"), factory);
        }
    }
}
