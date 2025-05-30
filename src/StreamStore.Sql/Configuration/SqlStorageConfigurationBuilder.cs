using System;
using Microsoft.Extensions.Configuration;
using StreamStore.Extensions;


namespace StreamStore.Sql.Configuration
{
    public class SqlStorageConfigurationBuilder
    {
        readonly SqlStorageConfiguration defaultConfig;
        readonly SqlStorageConfiguration configuration;
        public SqlStorageConfigurationBuilder(SqlStorageConfiguration defaultConfig)
        {
            this.defaultConfig = defaultConfig.ThrowIfNull(nameof(defaultConfig));
            configuration = (SqlStorageConfiguration)defaultConfig.Clone();
        }

        public SqlStorageConfigurationBuilder WithConnectionString(string connectionString)
        {
            configuration.ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            return this;
        }

        public SqlStorageConfigurationBuilder WithTable(string name)
        {
            configuration.TableName = name ?? throw new ArgumentNullException(nameof(name));
            return this;
        }

        public SqlStorageConfigurationBuilder WithSchema(string schema)
        {
            configuration.SchemaName = schema ?? throw new ArgumentNullException(nameof(schema));
            return this;
        }

        public SqlStorageConfiguration Build()
        {
            if (configuration.SchemaName == null)
            {
                throw new InvalidOperationException("SchemaName must be set");
            }

            if (configuration.TableName == null)
            {
                throw new InvalidOperationException("TableName string must be set");
            }

            return (SqlStorageConfiguration)configuration.Clone();
        }

        public SqlStorageConfiguration ReadFromConfig(IConfiguration configuration, string sectionName)
        {
            var section = configuration.GetSection(sectionName);
            if (section.Exists())
            {
                WithTable(section.GetValue("TableName", defaultConfig.TableName)!);
                WithSchema(section.GetValue("SchemaName", defaultConfig.SchemaName)!);
            }

            var connectionString = configuration.GetConnectionString("StreamStore");
            if (!string.IsNullOrWhiteSpace(connectionString)) WithConnectionString(connectionString);

            return Build();
        }
    }
}
