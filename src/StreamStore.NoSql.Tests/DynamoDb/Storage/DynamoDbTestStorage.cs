using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.NoSql.DynamoDb;
using StreamStore.Provisioning;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.DynamoDb.Storage
{
    public sealed class DynamoDbTestStorage: ITestStorage
    {
        readonly DynamoDbConfiguration config;
        const int readingBatchSize = 5;


        public DynamoDbTestStorage(DynamoDbConfiguration config)
        {
            this.config = config.ThrowIfNull(nameof(config));
        }

        public void Dispose()
        {

        }

        internal IServiceCollection ConfigurePersistence(IServiceCollection services)
        {
           return services.UseDynamoDb(c =>
                    c.UseConfiguration(
                        new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", true)
                        .AddJsonFile("appsettings.Development.json", true)
                        .Build())
                      .WithTableName(config.TableName)
                      .WithReadingBatchSize(readingBatchSize));
        }

        public bool EnsureExists()
        {
            try
            {
                var schemaProvisioner = ConfigurePersistence(new ServiceCollection()).BuildServiceProvider().GetRequiredService<ISchemaProvisioner>();
                schemaProvisioner.ProvisionSchemaAsync(CancellationToken.None).RunSynchronously();
                return true;
            }
            catch (AmazonClientException)
            {
                return false;
            }
        }

    }
}
