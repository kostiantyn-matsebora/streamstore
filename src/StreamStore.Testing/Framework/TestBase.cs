namespace StreamStore.Testing.Framework
{
    public abstract class TestBase<TSuite> where TSuite: ITestSuite
    {
        protected readonly TSuite Suite;

        protected TestBase(TSuite suite)
        {
            ArgumentNullException.ThrowIfNull(suite, nameof(suite));
            Suite = suite;
            Suite.SetUpSuite().Wait();
        }

        protected virtual void TrySkip()
        {
            Skip.IfNot(Suite.ArePrerequisitiesMet, "Suite is not ready.");
        }
    }
}
