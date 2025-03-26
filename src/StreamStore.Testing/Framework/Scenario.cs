using StreamStore.Testing.Framework;

namespace StreamStore.Testing
{
    public abstract class Scenario<TSuite> where TSuite : ITestSuite, new()
    {
        protected readonly TSuite Suite;

        protected Scenario() : this(new TSuite())
        {
        }

        protected Scenario(TSuite suite)
        {
            suite.ThrowIfNull(nameof(suite));
            Suite = suite;
            Suite.SetUpSuite();
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
