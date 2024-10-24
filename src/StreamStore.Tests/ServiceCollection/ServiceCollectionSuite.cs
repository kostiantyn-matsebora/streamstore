using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.Testing.Framework;

namespace StreamStore.Tests.ServiceCollection
{
    public class ServiceCollectionSuite: TestSuiteBase
    {

        public ServiceCollectionSuite()
        {
            this.MockRepository = new Moq.MockRepository(Moq.MockBehavior.Strict);
            this.MockServices = MockRepository.Create<IServiceCollection>();

        }

        public MockRepository MockRepository { get; }
        public Mock<IServiceCollection> MockServices { get; }
    }
}
