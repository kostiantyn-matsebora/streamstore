namespace StreamStore.S3.Storage
{
    interface IS3ObjectContainer<out T> where T : IS3Object
    {
        T GetChild(Id containerId);
    }
}
