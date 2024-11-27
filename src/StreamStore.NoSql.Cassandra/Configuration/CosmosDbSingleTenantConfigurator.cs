using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Cassandra;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.Database;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public class CosmosDbSingleTenantConfigurator: CassandraSingleTenantConfigurator<CosmosDbSingleTenantConfigurator>
    {
        string? contractPoint;
        RemoteCertificateValidationCallback validationCallback = ValidateServerCertificate;

        public CosmosDbSingleTenantConfigurator()
        {
            builder.WithPort(10350);
            mode = CassandraMode.CosmosDbCassandra;
        }

        public CosmosDbSingleTenantConfigurator WithCosmosDbAccount(string name)
        {
            contractPoint = $"{name}.cassandra.cosmos.azure.com";
            return this;
        }

        public CosmosDbSingleTenantConfigurator WithSSLValidationCallback(RemoteCertificateValidationCallback validationCallback)
        {

            this.validationCallback = validationCallback;
            return this;
        }

        protected override void ApplySpecificDependencies(IServiceCollection services)
        {
            if (!string.IsNullOrEmpty(contractPoint))
            {
                builder.AddContactPoint(contractPoint);
                ConfigureSSL();
            };

            base.ApplySpecificDependencies(services);
        }

        void ConfigureSSL()
        {
            var options = new SSLOptions(SslProtocols.Tls12, true, remoteCertValidationCallback: validationCallback);
            options.SetHostNameResolver((ipAddress) => contractPoint);
            builder.WithSSL(options);
        }

        internal static bool ValidateServerCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None) return true;

            return false;
        }
    }
}
