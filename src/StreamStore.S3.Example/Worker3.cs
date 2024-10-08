namespace StreamStore.S3.Example
{
    public  class Worker3: Worker
    {
        public Worker3(ILogger<Worker> logger, IStreamStore store) : base(logger, store)
        {
        }
    }
}
