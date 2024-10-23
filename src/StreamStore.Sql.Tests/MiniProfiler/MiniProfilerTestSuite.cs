using System.Data.Entity.Infrastructure;
using Moq;
using StreamStore.SQL;
using StreamStore.Testing.Framework;
using IDbConnectionFactory = StreamStore.SQL.IDbConnectionFactory;

namespace StreamStore.Sql.Tests.MiniProfiler
{
    public class MiniProfilerTestSuite: TestSuiteBase
    {

        public MiniProfilerTestSuite(): base()
        {
            DbConnectionFactory = new Mock<IDbConnectionFactory>();
            ProfilerDbConnectionFactory = new MiniProfilerDbConnectionFactory(DbConnectionFactory.Object);

        }

        public Mock<IDbConnectionFactory> DbConnectionFactory { get; set; }
        public MiniProfilerDbConnectionFactory ProfilerDbConnectionFactory { get; set; }


    }
}
