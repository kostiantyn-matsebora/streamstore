using System;
using Microsoft.Extensions.Configuration;
using StreamStore.Extensions;


namespace StreamStore.Sql.Configuration
{
    public class SqlStorageConfigurationBuilder
    {
        readonly SqlStorageConfiguration configuration;

        public SqlStorageConfigurationBuilder()
            : this(new SqlStorageConfiguration())
        {
        }

        public SqlStorageConfigurationBuilder(SqlStorageConfiguration defaultConfig, Action<SqlStorageConfigurationBuilder>? configure = null)
        {
            configuration = defaultConfig;
            if (configure != null)
            {
                configure(this);
            }
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

        public static SqlStorageConfiguration ReadFromConfig(IConfiguration configuration, string sectionName, SqlStorageConfiguration? defaultConfig = null)
        {
            configuration.ThrowIfNull(nameof(configuration));
            sectionName.ThrowIfNull(nameof(sectionName));

            var builder = new SqlStorageConfigurationBuilder();

            var config = defaultConfig ?? new SqlStorageConfiguration();

            var section = configuration.GetSection(sectionName);
            if (section.Exists())
            {
                builder.WithTable(section.GetValue("TableName", config.TableName)!);
                builder.WithSchema(section.GetValue("SchemaName", config.SchemaName)!);
            }

            var connectionString = configuration.GetConnectionString("StreamStore");
            if (!string.IsNullOrWhiteSpace(connectionString)) builder.WithConnectionString(connectionString);

            return builder.Build();
        }
    }
}
