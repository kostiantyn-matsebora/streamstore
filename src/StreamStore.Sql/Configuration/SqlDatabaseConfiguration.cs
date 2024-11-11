using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace StreamStore.Sql.Configuration
{
    public class SqlDatabaseConfiguration : ICloneable
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string SchemaName { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;

        public string FullTableName => $"{SchemaName}.{TableName}";

        public bool ProvisionSchema { get; set; } = true;

        public SqlDatabaseConfiguration()
        {
        }

        public object Clone()
        {
            return new SqlDatabaseConfiguration
            {
                SchemaName = SchemaName,
                TableName = TableName,
                ConnectionString = ConnectionString,
                ProvisionSchema = ProvisionSchema
            };
        }
    }

    public class SqlDatabaseConfigurationBuilder : IConfigurator
    {
        readonly SqlDatabaseConfiguration configuration;

        public SqlDatabaseConfigurationBuilder(SqlDatabaseConfiguration defaultConfig)
        {
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

        public SqlDatabaseConfigurationBuilder ProvisionSchema(bool provisionSchema)
        {
            configuration.ProvisionSchema = provisionSchema;
            return this;
        }

        public SqlDatabaseConfiguration Build()
        {
            if (configuration.ConnectionString == null)
            {
                throw new InvalidOperationException("Connection string must be set");
            }

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

        public virtual IServiceCollection Configure()
        {
            return new ServiceCollection();
        }

        public virtual IServiceCollection Configure(IConfiguration configuration, string sectionName)
        {
            return new ServiceCollection();
        }
    }
}
