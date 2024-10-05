using AutoFixture;
using StreamStore.S3.AmazonS3;

namespace StreamStore.S3.Tests
{
    public class S3StreamUnitOfWorkTests
    {
        public async Task SaveChangesAsync_ShouldSaveEvents()
        {
            var fixture = new Fixture();
            var records = fixture.CreateEventRecords(1);
            var settings =
                new S3StreamDatabaseSettingsBuilder()
                //.WithCredentials("xxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxx")
                //.WithEndpoint("xxxxxxxxxxxxxxxxxxxxxxxxx")
                .Build();

            var database = new S3StreamDatabase(new S3AbstractFactory(new AmazonClientFactory(settings), new AmazonLockFactory(settings)));

            await  database
                    .BeginAppend("stream-005")
                    .AddRange(records)
                    .SaveChangesAsync(CancellationToken.None);
        }
    }
}