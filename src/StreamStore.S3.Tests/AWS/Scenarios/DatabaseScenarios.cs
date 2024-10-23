using StreamStore.Testing.StreamDatabase.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.S3.Tests.AWS.Scenarios
{
    public class Finding_stream_metadata : Find_stream_data<AWSS3DatabaseSuite>
    {
        public Finding_stream_metadata() : base(new AWSS3DatabaseSuite())
        {
        }
    }
    public class Reading_from_database : Reading_from_database<AWSS3DatabaseSuite>
    {
        public Reading_from_database(ITestOutputHelper output) : base(new AWSS3DatabaseSuite(), output)
        {
        }
    }

    public class Deleting_from_database : Deleting_from_database<AWSS3DatabaseSuite>
    {
        public Deleting_from_database() : base(new AWSS3DatabaseSuite())
        {
        }
    }

    public class Writing_to_database : Writing_to_database<AWSS3DatabaseSuite>
    {
        public Writing_to_database() : base(new AWSS3DatabaseSuite())
        {
        }
    }
}
