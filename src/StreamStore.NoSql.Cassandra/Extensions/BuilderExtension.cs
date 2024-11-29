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
            if (!connectionBuilder.TryGetValue("hostname", out var hostName))
                throw new ArgumentException("Connection string does not contain hostname.");
            if (!connectionBuilder.TryGetValue("username", out var userName))
                throw new ArgumentException("Connection string does not contain username.");
            if (!connectionBuilder.TryGetValue("password", out var password))
                throw new ArgumentException("Connection string does not contain password.");
            if (!connectionBuilder.TryGetValue("port", out var port))
                throw new ArgumentException("Connection string does not contain port.");

            var options = new SSLOptions(SslProtocols.Tls12, true, remoteCertValidationCallback ?? ValidateServerCertificate);
            options.SetHostNameResolver((ipAddress) => hostName.ToString());

            builder
                .WithCredentials(userName.ToString(), password.ToString())
                .WithPort(Convert.ToInt32(port))
                .AddContactPoint(hostName.ToString())
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
