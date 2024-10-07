

namespace StreamStore.Testing
{
    public abstract class StreamDatabaseTests
    {
        readonly IStreamDatabase streamDatabase;

        protected StreamDatabaseTests(ITestSuite suite)
        {
            ArgumentNullException.ThrowIfNull(suite, nameof(suite));

            this.streamDatabase = suite.CreateDatabase();

        }


        [Fact]
        public async Task SaveChangesAsync_ShouldSaveChanges()
        {

        }
    }
}
