namespace StreamStore.S3.Example
{
    public  class Worker3: Worker
    {
        public Worker3(ILogger<Worker3> logger, IStreamStore store) : base(logger, store)
        {
        }
    }
}
