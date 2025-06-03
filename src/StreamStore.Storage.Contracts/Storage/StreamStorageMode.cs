namespace StreamStore.Storage
{
    public sealed class StreamStorageMode
    {
        bool multitenant;
        public static readonly StreamStorageMode Single = new StreamStorageMode(false);
        public static readonly StreamStorageMode Multitenant = new StreamStorageMode(true);

        StreamStorageMode(bool multitenant) 
        { 
            this.multitenant = multitenant;
        }
    }
}
