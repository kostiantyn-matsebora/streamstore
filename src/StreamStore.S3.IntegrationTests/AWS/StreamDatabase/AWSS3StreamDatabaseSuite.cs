using StreamStore.S3.AWS;
using StreamStore.Testing;
using StreamStore.Testing.StreamDatabase;

namespace StreamStore.S3.IntegrationTests.AWS.StreamDatabase
{
    public class AWSS3StreamDatabaseSuite : DatabaseSuiteBase
    {
        readonly S3IntegrationFixture? fixture;

        public override MemoryDatabase Container => fixture!.Container;

        public AWSS3StreamDatabaseSuite()
        {
        }

        public AWSS3StreamDatabaseSuite(S3IntegrationFixture fixture)
        {
            this.fixture = fixture;
        }

        protected override void ConfigureDatabase(IStreamStoreConfigurator configurator)
        {
            configurator.UseAWSDatabase();
        }

        protected override bool CheckPrerequisities()
        {
            return File.Exists(Path.Combine(AppContext.BaseDirectory, "appsettings.Development.json"));
        }

        protected override void SetUp()
        {
            fixture!.CopyTo(StreamDatabase);
        }
    }
}
