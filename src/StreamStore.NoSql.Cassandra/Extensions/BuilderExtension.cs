using System;
using System.Data.Common;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Cassandra;


namespace StreamStore.NoSql.Cassandra.Extensions
{

    internal static class BuilderExtension
    {
        public static Builder WithCosmosDbConnectionString(this Builder builder, string connectionString, RemoteCertificateValidationCallback? remoteCertValidationCallback = null)
        {

            var connectionBuilder = new DbConnectionStringBuilder();
            connectionBuilder.ConnectionString = connectionString;

            var contactPoint = connectionBuilder["hostname"].ThrowIfNull("hostname").ToString();

            var options = new SSLOptions(SslProtocols.Tls12, true, remoteCertValidationCallback ?? ValidateServerCertificate);

            options.SetHostNameResolver((ipAddress) => contactPoint);

            builder
                .WithCredentials(
                    username: connectionBuilder["username"].ThrowIfNull("username").ToString(),
                    password: connectionBuilder["password"].ThrowIfNull("password").ToString())
                .WithPort(Convert.ToInt32(connectionBuilder["port"].ThrowIfNull("port")))
                .AddContactPoint(contactPoint)
                .WithSSL(options);

            return builder;
        }

        static bool ValidateServerCertificate(
#pragma warning disable S1172 // Unused method parameters should be removed
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
#pragma warning restore S1172 // Unused method parameters should be removed
        {
            if (sslPolicyErrors == SslPolicyErrors.None) return true;

            return false;
        }
    }
}
