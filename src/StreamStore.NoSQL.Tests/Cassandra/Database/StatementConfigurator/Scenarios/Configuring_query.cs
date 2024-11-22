using Cassandra;
using FluentAssertions;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Database.QueryConfigurator
{
    public class Configuring_query : Scenario
    {

        [Fact]
        public void When_any_of_parameters_is_not_set()
        {

            // Act
            var act = () => new CassandraStatementConfigurator(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_query_is_configured()
        {
            // Arrange
            var config = new CassandraStorageConfiguration()
            {
                ReadConsistencyLevel = ConsistencyLevel.Three,
                SerialConsistencyLevel = ConsistencyLevel.LocalSerial
            };
            var statement = new SimpleStatement();
            var configurator = new CassandraStatementConfigurator(config);

            // Act
            configurator.ConfigureQuery<SimpleStatement>(statement);

            // Assert
            statement.ConsistencyLevel.Should().Be(config.ReadConsistencyLevel);
            statement.SerialConsistencyLevel.Should().Be(config.SerialConsistencyLevel);
        }
    }
}
