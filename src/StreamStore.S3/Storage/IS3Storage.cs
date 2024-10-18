namespace StreamStore.S3.Storage
{
    internal interface IS3Storage
    {
        S3StreamStorage Persistent { get; }
        S3StreamStorage Transient { get; }
        S3LockStorage Locks { get; }

    }
}
