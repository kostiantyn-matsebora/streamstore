
//using FluentAssertions;
//using StreamStore.Testing;
//using StreamStore.InMemory;
//using Microsoft.Extensions.DependencyInjection;
//using StreamStore.Provisioning;
//using static StreamStore.Tests.Configuration.SingleTenant.SingleTenantConfiguratorTestEnvironment;
//using StreamStore.InMemory.Extensions;

//namespace StreamStore.Tests.Configuration.SingleTenant
//{
//    public class Configuring_single_storage: Scenario<SingleTenantConfiguratorTestEnvironment>
//    {

//        [Fact]
//        public void When_storage_is_not_configured()
//        {
//            // Arrange
//            var configurator = CreateConfigurator();

//            // Act
//            var act = () => configurator.Configure();

//            //Assert
//            act.Should().Throw<InvalidOperationException>().WithMessage("Storage backend (IStreamStorage) is not registered");
//        }

//        [Fact]
//        public void When_storage_is_configured_by_defaults()
//        {
//            // Arrange
//            var configurator = CreateConfigurator();

//            // Act
//            configurator.UseInMemoryStorage();
//            var services =  configurator.Configure();

//            //Assert
//            var provider = services.BuildServiceProvider();

//            provider.GetRequiredService<IStreamStorage>()
//                     .Should().NotBeNull()
//                     .And.BeOfType<InMemoryStreamStorage>();

//            provider.GetRequiredService<IStreamReader>()
//                        .Should().NotBeNull()
//                        .And.BeOfType<InMemoryStreamStorage>();

//            provider.GetRequiredService<ISchemaProvisioner>()
//                    .Should().NotBeNull()
//                    .And.BeOfType<DefaultSchemaProvisioner>();
//        }


//        [Fact]
//        public void When_storage_is_configured_by_storage_instance()
//        {
//            // Arrange
//            var configurator = CreateConfigurator();
//            var storage = MockStreamStorage.Object;

//            // Act
//            configurator.UseStorage(storage);
//            var services = configurator.Configure();

//            //Assert
//            var provider = services.BuildServiceProvider();

//            provider.GetRequiredService<IStreamStorage>()
//                     .Should().NotBeNull()
//                     .And.Be(storage);

//            provider.GetRequiredService<IStreamReader>()
//                        .Should().NotBeNull()
//                        .And.Be(storage);

//            provider.GetRequiredService<ISchemaProvisioner>()
//                    .Should().NotBeNull()
//                    .And.BeOfType<DefaultSchemaProvisioner>();
//        }


//        [Fact]
//        public void When_storage_is_configured_with_custom_dependencies()
//        {
//            // Arrange
//            var configurator = CreateConfigurator();


//            // Act
//            configurator
//                .UseInMemoryStorage()
//                .UseSchemaProvisioner<FakeSchemaProvisioner>();
//            var services = configurator.Configure();

//            //Assert
//            var provider = services.BuildServiceProvider();
//            provider.GetRequiredService<ISchemaProvisioner>()
//                     .Should().NotBeNull()
//                     .And.BeOfType<FakeSchemaProvisioner>();
//        }
//    }
//}
