using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.NoSql.DynamoDb;
using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.DynamoDb.Storage
{
    public class DynamoDbStorageFixture : StorageFixtureBase<DynamoDbTestStorage>
    {

        public DynamoDbStorageFixture() : this(new DynamoDbTestStorage(new DynamoDbConfiguration { TableName = Generated.Names.Storage }))
        {
        }

        DynamoDbStorageFixture(DynamoDbTestStorage storage) : base(storage)
        {
        }


        public override void ConfigurePersistence(IServiceCollection services)
        {
          testStorage.ConfigurePersistence(services);
        }

        protected override InMemoryStreamContainer CreateInMemoryContainer()
        {
            return new InMemoryStreamContainer(new InMemoryStreamContainerOptions { Capacity = 20, EventPerStream = 33 });
        }
    }
}
