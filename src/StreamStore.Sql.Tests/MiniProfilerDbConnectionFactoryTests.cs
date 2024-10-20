using FluentAssertions;
using Moq;
using StackExchange.Profiling.Data;
using StreamStore.SQL;
using System;
using Xunit;

namespace StreamStore.Sql.Tests
{
    public class MiniProfilerDbConnectionFactoryTests
    {
        readonly MockRepository mockRepository;

        readonly Mock<IDbConnectionFactory> mockDbConnectionFactory;

        public MiniProfilerDbConnectionFactoryTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            mockDbConnectionFactory = this.mockRepository.Create<IDbConnectionFactory>();
        }

        MiniProfilerDbConnectionFactory CreateFactory()
        {
            return new MiniProfilerDbConnectionFactory(
                this.mockDbConnectionFactory.Object);
        }

        [Fact]
        public void GetConnection_Should_ReturnProfilerConnection()
        {
            // Arrange
            var factory = this.CreateFactory();

            mockDbConnectionFactory.Setup(x => x.GetConnection()).Returns(new System.Data.SqlClient.SqlConnection());

            // Act
            var result = factory.GetConnection();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProfiledDbConnection>();
            mockRepository.VerifyAll();
        }
    }
}
