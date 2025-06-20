﻿using FluentAssertions;
using Moq;
using StreamStore.Multitenancy;
using StreamStore.Testing;
using Environment = StreamStore.Tests.StreamStore.Multitenancy.TenantStreamStoreFactoryTestEnvironment;

namespace StreamStore.Tests.StreamStore.Multitenancy
{
    public class Creating_tenant_stream_store : Scenario<TenantStreamStoreFactoryTestEnvironment>
    {

        [Fact]
        public void When_one_of_the_parameters_is_null()
        {

            // Act
            var act = () => new TenantStreamStoreFactory(null!, Environment.MockTenantStreamStorageProvider.Object, Environment.MockEventConverter.Object, Environment.MockStreamMutationRequestValidator.Object);

            //Assert
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("configuration");

            // Act
            act = () => new TenantStreamStoreFactory(Environment.Configuration, null!, Environment.MockEventConverter.Object, Environment.MockStreamMutationRequestValidator.Object);

            //Assert
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("storageProvider");

            // Act
            act = () => new TenantStreamStoreFactory(Environment.Configuration, Environment.MockTenantStreamStorageProvider.Object, null!, Environment.MockStreamMutationRequestValidator.Object);

            //Assert
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("converter");

            // Act
            act = () => new TenantStreamStoreFactory(Environment.Configuration, Environment.MockTenantStreamStorageProvider.Object, Environment.MockEventConverter.Object, null!);

            //Assert
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("validator");

        }

        [Fact]
        public void When_streamstore_is_created()
        {
            // Arrange
            var storageProvider = Environment.MockTenantStreamStorageProvider;
            storageProvider.Setup(x => x.GetStorage(It.IsAny<Id>())).Returns(Generated.Mocks.Single<IStreamStorage>().Object);
            var factory = new TenantStreamStoreFactory(Environment.Configuration, storageProvider.Object, Environment.MockEventConverter.Object, Environment.MockStreamMutationRequestValidator.Object);

            // Act
            var store = factory.Create(Generated.Primitives.Id);

            //Assert
            store.Should().NotBeNull();
        }
    }
}
