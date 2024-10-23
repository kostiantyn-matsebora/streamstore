using StreamStore.Testing.Framework;

namespace StreamStore.Testing
{
    public abstract class Scenario<TSuite> where TSuite: ITestSuite
    {
        protected readonly TSuite Suite;

        protected Scenario(TSuite suite)
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

    public class Scenario: Scenario<TestSuite>
    {
        public Scenario() : base(new TestSuite())
        {
        }
    }
}
