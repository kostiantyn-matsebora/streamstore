namespace StreamStore.Testing.Framework
{
    public abstract class TestsBase<TSuite> where TSuite: ITestSuite
    {
        protected readonly TSuite Suite;

        protected TestsBase(TSuite suite)
        {
            ArgumentNullException.ThrowIfNull(suite, nameof(suite));
            this.Suite = suite;
            this.Suite.SetUpSuite().Wait();
        }

        protected virtual void TrySkip()
        {
            Skip.IfNot(Suite.ArePrerequisitiesMet, "Suite is not ready.");
        }
    }
}
