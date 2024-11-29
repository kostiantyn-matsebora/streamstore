using System;
using Microsoft.Extensions.Configuration;


namespace StreamStore.Sql.Configuration
{
    public class SqlDatabaseConfigurationBuilder
    {
        readonly SqlDatabaseConfiguration defaultConfig;
        readonly SqlDatabaseConfiguration configuration;
        public SqlDatabaseConfigurationBuilder(SqlDatabaseConfiguration defaultConfig)
        {
            this.defaultConfig = defaultConfig.ThrowIfNull(nameof(defaultConfig));
            configuration = (SqlDatabaseConfiguration)defaultConfig.Clone();
        }

        public SqlDatabaseConfigurationBuilder WithConnectionString(string connectionString)
        {
            configuration.ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            return this;
        }

        public SqlDatabaseConfigurationBuilder WithTable(string name)
        {
            configuration.TableName = name ?? throw new ArgumentNullException(nameof(name));
            return this;
        }

        public SqlDatabaseConfigurationBuilder WithSchema(string schema)
        {
            configuration.SchemaName = schema ?? throw new ArgumentNullException(nameof(schema));
            return this;
        }

        public SqlDatabaseConfiguration Build()
        {
            if (configuration.SchemaName == null)
            {
                throw new InvalidOperationException("SchemaName must be set");
            }

            if (configuration.TableName == null)
            {
                throw new InvalidOperationException("TableName string must be set");
            }

            return (SqlDatabaseConfiguration)configuration.Clone();
        }

        public SqlDatabaseConfiguration ReadFromConfig(IConfiguration configuration, string sectionName)
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
