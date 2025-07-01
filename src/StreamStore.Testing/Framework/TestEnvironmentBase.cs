
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace StreamStore.Testing.Framework
{
    public abstract class TestEnvironmentBase : ITestEnvironment
    {
        IServiceProvider? serviceProvider;
        bool isReady;

        public IServiceProvider Services => serviceProvider!;

        public bool IsReady => isReady;

        public async Task SetUp()
        {
            isReady = CheckIfReady();
            if (!isReady) return;

            var services = new ServiceCollection();

            RegisterServices(services);
            BuildProvider(services);
            await SetUpAsync();
        }


        protected TestEnvironmentBase()
        {
        }

        protected virtual bool CheckIfReady()
        {
            return true;
        }

        protected virtual void RegisterServices(IServiceCollection services)
        {
        }

        protected virtual Task SetUpAsync()
        {
            return Task.CompletedTask;
        }


        void BuildProvider(IServiceCollection services)
        {
            serviceProvider = services.BuildServiceProvider();
        }
    }

    public class TestEnvironment : TestEnvironmentBase
    {
        public MockRepository MockRepository { get; }

        public TestEnvironment()
        {
            MockRepository = new MockRepository(MockBehavior.Strict);
        }
    }
}
