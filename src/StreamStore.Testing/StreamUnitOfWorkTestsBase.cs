


namespace StreamStore.Testing
{
    public abstract class StreamUnitOfWorkTestsBase
    {
        readonly ITestSuite suite;

        protected StreamUnitOfWorkTestsBase(ITestSuite suite)
        {
            ArgumentNullException.ThrowIfNull(suite, nameof(suite));
            this.suite = suite;
        }
    }
}
