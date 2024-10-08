namespace StreamStore.Testing
{
    public abstract class DatabaseTestsBase
    {

        protected readonly ITestSuite suite;

        protected DatabaseTestsBase(ITestSuite suite)
        {
            ArgumentNullException.ThrowIfNull(suite, nameof(suite));
            this.suite = suite;
        }

        protected virtual void TrySkip()
        {
            Skip.If(suite.CreateDatabase() == null, "Database is not set.");
        }

    }
}
