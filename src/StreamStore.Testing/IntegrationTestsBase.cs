namespace StreamStore.Testing
{
    public abstract class IntegrationTestsBase
    {

        protected readonly ITestSuite suite;

        protected IntegrationTestsBase(ITestSuite suite)
        {
            ArgumentNullException.ThrowIfNull(suite, nameof(suite));
            this.suite = suite;
            this.suite.Initialize();
        }

        protected virtual void TrySkip()
        {
            Skip.If(!suite.IsReady, "Suite is not ready.");
        }
    }
}
