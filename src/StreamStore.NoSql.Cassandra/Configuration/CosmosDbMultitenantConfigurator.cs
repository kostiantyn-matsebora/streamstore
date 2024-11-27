using System.Net.Security;
using System.Security.Authentication;
using Cassandra;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.Database;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public class CosmosDbMultitenantConfigurator : CassandraMultitenantConfigurator<CosmosDbMultitenantConfigurator>
    {
        string? contractPoint;
        RemoteCertificateValidationCallback validationCallback = CosmosDbSingleTenantConfigurator.ValidateServerCertificate;

        public CosmosDbMultitenantConfigurator()
        {

            clusterConfigurator!.AddConfigurator(builder => builder.WithPort(10350));
            mode = CassandraMode.CosmosDbCassandra;
        }

        public CosmosDbMultitenantConfigurator WithCosmosDbAccount(string name)
        {
            contractPoint = $"{name}.cassandra.cosmos.azure.com";
            return this;
        }

        public CosmosDbMultitenantConfigurator WithSSLValidationCallback(RemoteCertificateValidationCallback validationCallback)
        {

            this.validationCallback = validationCallback;
            return this;
        }

        protected override void ApplySpecificDependencies(IServiceCollection services)
        {
            if (!string.IsNullOrEmpty(contractPoint))
            {
                clusterConfigurator!.AddConfigurator(builder =>
                {
                    builder.AddContactPoint(contractPoint);
                    ConfigureSSL(builder);
                });
            };

            base.ApplySpecificDependencies(services);
        }

        void ConfigureSSL(Builder builder)
        {
            var options = new SSLOptions(SslProtocols.Tls12, true, remoteCertValidationCallback: validationCallback);
            options.SetHostNameResolver((ipAddress) => contractPoint);
            builder.WithSSL(options);
        }
    }
}
