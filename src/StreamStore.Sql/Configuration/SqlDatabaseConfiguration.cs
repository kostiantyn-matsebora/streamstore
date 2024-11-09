using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace StreamStore.Sql { 
    public class SqlDatabaseConfiguration
    {
        public string ConnectionString { get;  set; } = string.Empty;
        public string SchemaName { get;  set; } = string.Empty;
        public string TableName { get;  set; } = string.Empty;

        public string FullTableName => $"{SchemaName}.{TableName}";

        public bool ProvisionSchema { get; set; } = true;

        public SqlDatabaseConfiguration()
        {
        }
    }

    public class SqlDatabaseConfigurationBuilder : IConfigurator
    {
        string? connectionString;
        string name = "Events";
        string schema = "main";
        bool provisionSchema = true;

        public SqlDatabaseConfigurationBuilder WithConnectionString(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            return this;
        }

        public SqlDatabaseConfigurationBuilder WithTable(string name)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            return this;
        }

        public SqlDatabaseConfigurationBuilder WithSchema(string schema)
        {
            this.schema = schema ?? throw new ArgumentNullException(nameof(schema));
            return this;
        }

        public SqlDatabaseConfigurationBuilder ProvisionSchema(bool provisionSchema)
        {
            this.provisionSchema = provisionSchema;
            return this;
        }

        public SqlDatabaseConfiguration Build()
        {
            if (connectionString == null)
            {
                throw new InvalidOperationException("Connection string must be set");
            }
            return new SqlDatabaseConfiguration
            {
                ConnectionString = connectionString,
                SchemaName = schema,
                TableName = name,
                ProvisionSchema = provisionSchema
            };
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
