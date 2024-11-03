namespace StreamStore.S3.Tests.Integration.B2.StreamDatabase
{
    [CollectionDefinition("Reading B2")]
    public class Reading_collection : ICollectionFixture<S3IntegrationFixture>
    {
        public Reading_collection() : base()
        {
        }
    }

    [CollectionDefinition("Deleting B2")]
    public class Deleting_collection : ICollectionFixture<S3IntegrationFixture>
    {
        public Deleting_collection() : base()
        {
        }
    }

    [CollectionDefinition("Writing B2")]
    public class Writing_collection : ICollectionFixture<S3IntegrationFixture>
    {
        public Writing_collection() : base()
        {
        }
    }
}
