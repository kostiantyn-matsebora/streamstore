
using System;
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

        public void SetUp()
        {
            isReady = CheckIfReady();
            if (!isReady) return;

            var services = new ServiceCollection();

            RegisterServices(services);
            BuildProvider(services);
            SetUpInternal();
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

        protected virtual void SetUpInternal()
        {
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
