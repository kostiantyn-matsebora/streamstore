namespace StreamStore.S3.Example
{
    public  class Worker2: Worker
    {
        public Worker2(ILogger<Worker> logger, IStreamStore store) : base(logger, store)
        {
        }
    }
}
