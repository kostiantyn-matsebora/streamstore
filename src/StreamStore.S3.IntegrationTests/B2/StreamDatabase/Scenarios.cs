using StreamStore.S3.IntegrationTests;
using StreamStore.Testing.StreamDatabase.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.S3.IntegrationTests.B2.StreamDatabase
{
    public class Scenarios
    {
        [Collection("Reading B2")]
        public class Finding_stream_metadata : Find_stream_data<B2S3StreamDatabaseSuite>
        {
            public Finding_stream_metadata(S3IntegrationFixture fixture) : base(new B2S3StreamDatabaseSuite(fixture))
            {
            }
        }

        [Collection("Reading B2")]
        public class Reading_from_database : Reading_from_database<B2S3StreamDatabaseSuite>
        {
            public Reading_from_database(S3IntegrationFixture fixture, ITestOutputHelper output) : base(new B2S3StreamDatabaseSuite(fixture), output)
            {
            }
        }

        [Collection("Deleting B2")]
        public class Deleting_from_database : Deleting_from_database<B2S3StreamDatabaseSuite>
        {
            public Deleting_from_database(S3IntegrationFixture fixture) : base(new B2S3StreamDatabaseSuite(fixture))
            {
            }
        }

        [Collection("Writing B2")]
        public class Writing_to_database : Writing_to_database<B2S3StreamDatabaseSuite>
        {
            public Writing_to_database(S3IntegrationFixture fixture) : base(new B2S3StreamDatabaseSuite(fixture))
            {
            }
        }
    }
}
