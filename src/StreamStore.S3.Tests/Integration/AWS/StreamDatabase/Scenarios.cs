using StreamStore.S3.Tests.Integration.AWS.StreamDatabase;
using StreamStore.Testing.StreamDatabase.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.S3.Tests.Integration.AWS.Database
{
    public class Finding_stream_metadata : Find_stream_data<AWSS3StreamDatabaseSuite>
    {
        public Finding_stream_metadata() : base(new AWSS3StreamDatabaseSuite())
        {
        }
    }
    public class Reading_from_database : Reading_from_database<AWSS3StreamDatabaseSuite>
    {
        public Reading_from_database(ITestOutputHelper output) : base(new AWSS3StreamDatabaseSuite(), output)
        {
        }
    }

    public class Deleting_from_database : Deleting_from_database<AWSS3StreamDatabaseSuite>
    {
        public Deleting_from_database() : base(new AWSS3StreamDatabaseSuite())
        {
        }
    }

    public class Writing_to_database : Writing_to_database<AWSS3StreamDatabaseSuite>
    {
        public Writing_to_database() : base(new AWSS3StreamDatabaseSuite())
        {
        }
    }
}
