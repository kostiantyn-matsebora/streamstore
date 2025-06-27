using System;
using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Storage.Configuration;

namespace StreamStore.NoSql.DynamoDb.Configuration
{
    internal class StorageConfigurator : StorageConfiguratorBase, IDynamoDbConfigurator
    {
        readonly DynamoDbConfiguration configuration;
        Func<IServiceProvider, IAmazonDynamoDB>? clientFactory;
        AWSOptions? options;
        
        public StorageConfigurator()
        {
            this.configuration = DynamoDbConfiguration.Default;
        }

        protected override void ConfigureAdditionalDependencies(IServiceCollection services)
        {
            services.AddSingleton(configuration);

            if (options != null)
                services.AddDefaultAWSOptions(options);

            if (clientFactory != null)
            {
                services.AddSingleton<IAmazonDynamoDB>(clientFactory);
            }
            else
            {
                services.AddAWSService<IAmazonDynamoDB>();
            }

            services.AddSingleton<DynamoDbRequests>();
        }

        protected override void ConfigureSchemaProvisioner(SchemaProvisionerRegistrator registrator)
        {
            registrator.RegisterSchemaProvisioner<DynamoDbSchemaProvisioner>();
        }

        protected override void ConfigureStorage(StorageDependencyRegistrator registrator)
        {
           registrator.RegisterStorage<DynamoDbStreamStorage>();
        }

        public IDynamoDbConfigurator UseConfiguration(IConfiguration configuration, string configSection = "streamstore")
        {
            configuration.ThrowIfNull(nameof(configuration));
            configSection.ThrowIfNull(nameof(configSection));

            options = configuration.GetAWSOptions(configSection);

            return this;
        }

        public IDynamoDbConfigurator WithTableName(string tableName)
        {
            this.configuration.TableName = tableName.ThrowIfNull(nameof(tableName));
            return this;
        }

        public IDynamoDbConfigurator WithBillingMode(BillingMode mode)
        {
            this.configuration.BillingMode = mode.ThrowIfNull(nameof(mode));
            return this;
        }

        public IDynamoDbConfigurator UseClientFactory(Func<IServiceProvider, IAmazonDynamoDB> clientFactory)
        { 
            this.clientFactory = clientFactory.ThrowIfNull(nameof(clientFactory));
            return this; 
        }
    }
}
