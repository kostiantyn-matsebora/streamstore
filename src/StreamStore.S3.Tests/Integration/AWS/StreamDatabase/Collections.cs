namespace StreamStore.S3.Tests.Integration.AWS.StreamDatabase
{
    [CollectionDefinition("Reading AWS")]
    public class Reading_collection : ICollectionFixture<S3IntegrationFixture>
    {
        public Reading_collection() : base()
        {
        }
    }

    [CollectionDefinition("Deleting AWS")]
    public class Deleting_collection : ICollectionFixture<S3IntegrationFixture>
    {
        public Deleting_collection() : base()
        {
        }
    }

    [CollectionDefinition("Writing AWS")]
    public class Writing_collection : ICollectionFixture<S3IntegrationFixture>
    {
        public Writing_collection() : base()
        {
        }
    }
}
