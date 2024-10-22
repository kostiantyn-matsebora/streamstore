
using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.Testing.Framework
{
    public abstract class TestSuiteBase : ITestSuite
    {
        IServiceProvider? serviceProvider;
        bool prerequisitesAreMet;

        public IServiceProvider Services => serviceProvider!;

        public bool ArePrerequisitiesMet => prerequisitesAreMet;

        public async Task SetUpSuite()
        {
            prerequisitesAreMet = CheckPrerequisities();
            if (!prerequisitesAreMet) return;

            var services = new ServiceCollection();

            RegisterServices(services);
            BuildProvider(services);
            await SetUp();
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

        protected virtual Task SetUp()
        {
            return Task.CompletedTask;
        }


        void BuildProvider(IServiceCollection services)
        {
            serviceProvider = services.BuildServiceProvider();
        }
    }
}
