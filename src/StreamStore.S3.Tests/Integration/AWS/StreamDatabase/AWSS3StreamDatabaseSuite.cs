using StreamStore.S3.AWS;
using StreamStore.Testing.StreamDatabase;

namespace StreamStore.S3.Tests.Integration.AWS.StreamDatabase
{
    public class AWSS3StreamDatabaseSuite : DatabaseSuiteBase
    {
        protected override void ConfigureDatabase(IStreamStoreConfigurator configurator)
        {
            configurator.UseAWSDatabase();
        }

        protected override bool CheckPrerequisities()
        {
            return File.Exists(Path.Combine(AppContext.BaseDirectory, "appsettings.Development.json"));
        }
    }
}
