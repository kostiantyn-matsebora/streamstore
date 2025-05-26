using System.Reflection;
using StreamStore.Sql.Migrations;

namespace StreamStore.Sql.Configuration
{
    public class MigrationConfiguration
    {
        public Assembly MigrationAssembly { get; set; } = typeof(Initial).Assembly;
    }
}
