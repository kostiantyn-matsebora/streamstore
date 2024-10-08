namespace StreamStore.S3.Example
{
    public  class Worker2: Worker
    {
        public Worker2(ILogger<Worker2> logger, IStreamStore store) : base(logger, store)
        {
        }
    }
}
