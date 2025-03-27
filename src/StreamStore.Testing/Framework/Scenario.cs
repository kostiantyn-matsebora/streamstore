using StreamStore.Testing.Framework;

namespace StreamStore.Testing
{
    public abstract class Scenario<TEnvironment> where TEnvironment : ITestEnvironment, new()
    {
        protected readonly TEnvironment Environment;

        protected Scenario() : this(new TEnvironment())
        {
        }

        protected Scenario(TEnvironment environment)
        {
            environment.ThrowIfNull(nameof(environment));
            Environment = environment;
            Environment.SetUp();
        }

        protected virtual void TrySkip()
        {
            Skip.IfNot(Environment.IsReady, "Environment is not ready.");
        }
    }

    public class Scenario: Scenario<TestEnvironment>
    {
        public Scenario() : base(new TestEnvironment())
        {
        }
    }
}
