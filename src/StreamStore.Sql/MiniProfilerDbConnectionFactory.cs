using System;
using System.Data.Common;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace StreamStore.SQL
{
    public sealed class MiniProfilerDbConnectionFactory : IDbConnectionFactory
    {
        private readonly IDbConnectionFactory factory;

        public MiniProfilerDbConnectionFactory(IDbConnectionFactory factory)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }
        public DbConnection GetConnection()
        {
            return new ProfiledDbConnection(factory.GetConnection(), MiniProfiler.Current);
        }
    }
}
