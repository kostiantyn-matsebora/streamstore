using FluentAssertions;
using StackExchange.Profiling.Data;
using StreamStore.Testing;

namespace StreamStore.Sql.Tests.MiniProfiler
{
    public class Getting_connection : Scenario<MiniProfilerTestSuite>
    {
        public Getting_connection() : base(new MiniProfilerTestSuite())
        {
        }

        [SkippableFact]
        public void When_internal_connection_exists()
        {
            TrySkip();

            // Arrange

            Suite.DbConnectionFactory.Setup(x => x.GetConnection()).Returns(new System.Data.SqlClient.SqlConnection());

            // Act
            var result = Suite.ProfilerDbConnectionFactory.GetConnection();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProfiledDbConnection>();
        }
    }
}
