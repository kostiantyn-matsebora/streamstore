using StreamStore.Testing.StreamDatabase.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.S3.Tests.Integration.B2.StreamDatabase
{
    public class Scenarios
    {
        public class Finding_stream_metadata : Find_stream_data<B2S3StreamDatabaseSuite>
        {
            public Finding_stream_metadata() : base(new B2S3StreamDatabaseSuite())
            {
            }
        }
        public class Reading_from_database : Reading_from_database<B2S3StreamDatabaseSuite>
        {
            public Reading_from_database(ITestOutputHelper output) : base(new B2S3StreamDatabaseSuite(), output)
            {
            }
        }

        public class Deleting_from_database : Deleting_from_database<B2S3StreamDatabaseSuite>
        {
            public Deleting_from_database() : base(new B2S3StreamDatabaseSuite())
            {
            }
        }

        public class Writing_to_database : Writing_to_database<B2S3StreamDatabaseSuite>
        {
            public Writing_to_database() : base(new B2S3StreamDatabaseSuite())
            {
            }
        }
    }
}
