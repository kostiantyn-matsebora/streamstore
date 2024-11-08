using StreamStore.S3.IntegrationTests;
using StreamStore.Testing.StreamDatabase.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.S3.IntegrationTests.AWS.StreamDatabase
{
    [Collection("Reading AWS")]
    public class Finding_stream_metadata : Find_stream_data<AWSS3StreamDatabaseSuite>
    {
        public Finding_stream_metadata(S3IntegrationFixture fixture) : base(new AWSS3StreamDatabaseSuite(fixture))
        {
        }
    }

    [Collection("Reading AWS")]
    public class Reading_from_database : Reading_from_database<AWSS3StreamDatabaseSuite>
    {
        public Reading_from_database(S3IntegrationFixture fixture, ITestOutputHelper output) : base(new AWSS3StreamDatabaseSuite(fixture), output)
        {
        }
    }

    [Collection("Deleting AWS")]
    public class Deleting_from_database : Deleting_from_database<AWSS3StreamDatabaseSuite>
    {
        public Deleting_from_database(S3IntegrationFixture fixture) : base(new AWSS3StreamDatabaseSuite(fixture))
        {
        }
    }

    [Collection("Writing AWS")]
    public class Writing_to_database : Writing_to_database<AWSS3StreamDatabaseSuite>
    {
        public Writing_to_database(S3IntegrationFixture fixture) : base(new AWSS3StreamDatabaseSuite(fixture))
        {
        }
    }
}
