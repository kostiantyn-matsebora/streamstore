﻿using Cassandra;
using Cassandra.Mapping;
using Moq;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Database.StatementConfigurator
{
    public class Configuring_batch: Scenario
    {
        [Fact]
        public void When_batch_is_configured()
        {
            // Arrange
            var config = new CassandraStorageConfiguration()
            {
                WriteConsistencyLevel = ConsistencyLevel.Three,
                SerialConsistencyLevel = ConsistencyLevel.LocalSerial
            };
            var batch = Suite.MockRepository.Create<ICqlBatch>();

            batch.Setup(b => b.WithOptions(It.IsAny<Action<CqlQueryOptions>>())).Returns(batch.Object);
            var configurator = new CassandraStatementConfigurator(config);

            // Act
            configurator.Batch(batch.Object);

            // Assert
            Suite.MockRepository.VerifyAll();
        }
    }
}