
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace StreamStore.Testing.Framework
{
    public abstract class TestSuiteBase : ITestSuite
    {
        IServiceProvider? serviceProvider;
        bool prerequisitesAreMet;

        public IServiceProvider Services => serviceProvider!;

        public bool ArePrerequisitiesMet => prerequisitesAreMet;

        public void SetUpSuite()
        {
            prerequisitesAreMet = CheckPrerequisities();
            if (!prerequisitesAreMet) return;

            var services = new ServiceCollection();

            RegisterServices(services);
            BuildProvider(services);
            SetUp();
        }


        protected TestSuiteBase()
        {
        }

        protected virtual bool CheckPrerequisities()
        {
            return true;
        }

        protected virtual void RegisterServices(IServiceCollection services)
        {
        }

        protected virtual void SetUp()
        {
        }


        void BuildProvider(IServiceCollection services)
        {
            serviceProvider = services.BuildServiceProvider();
        }
    }

    public class TestSuite : TestSuiteBase
    {
        public MockRepository MockRepository { get; }
        public TestSuite()
        {
            MockRepository = new MockRepository(MockBehavior.Strict);
        }
    }
}
