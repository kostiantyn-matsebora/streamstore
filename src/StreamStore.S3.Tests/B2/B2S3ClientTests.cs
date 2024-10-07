namespace StreamStore.S3.Tests.B2
{
    public  class B2S3ClientTests : S3ClientTestsBase
    {
        public B2S3ClientTests() : base(B2TestsSuite.CreateFactory())
        {
        }
    
    }
}
