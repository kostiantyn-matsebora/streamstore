//using StreamStore.Testing;
//using Microsoft.Extensions.DependencyInjection;
//using StreamStore.Sql.Configuration;
//using FluentAssertions;
//using StreamStore.Sql.API;
//using StreamStore.Sql.Sqlite;
//using StreamStore.Configuration.Storage;
//using StreamStore.Configuration;

//namespace StreamStore.Sql.Tests.Sqlite.Configuration.Configurator
//{
//    public class Configuring_storage : Scenario
//    {
//        [Fact]
//        public void When_configuring_streamstore_storage()
//        {
//            // Arrange
//            var configurator = ConfiguratorFactory.SingleTenantConfigurator;
//            var connectionString = Generated.Primitives.String;


//            // Act
//            var serviceCollection = configurator
//                    .UseSqliteStorage(c => c.ConfigureStreamStorage(x => x.WithConnectionString(connectionString)))
//                    .Configure();


//            var provider = serviceCollection.BuildServiceProvider();

//            // Assert
//            var configuration = provider.GetRequiredService<SqlStorageConfiguration>();
//            configuration.ConnectionString.Should().Be(connectionString);
//            configuration.SchemaName.Should().Be("main");
//            configuration.TableName.Should().Be("Events");
//            provider.GetService<IDbConnectionFactory>().Should().BeOfType<SqliteDbConnectionFactory>();
//            provider.GetService<ISqlExceptionHandler>().Should().BeOfType<SqliteExceptionHandler>();
//        }
//    }
//}
