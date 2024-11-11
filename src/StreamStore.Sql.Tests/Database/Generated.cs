
using AutoFixture;

namespace StreamStore.Sql.Tests.Database
{
    public static class Generated
    {
        public static string DatabaseName => "test_" + new Fixture().Create<string>().Replace("-", "_");
    }
}
