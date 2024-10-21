using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing.Framework;
using StreamStore.InMemory;
using StreamStore.Testing;
using AutoFixture;
namespace StreamStore.Tests.Scenarios
{
    public class StreamStoreSuite: TestSuiteBase
    {
        StreamStore? store;
        Id[]? streamIdentifiers;

        public IStreamStore Store => store!;
        public Id[] StreamIdentifiers => streamIdentifiers!;

        protected override void RegisterServices(IServiceCollection services)
        {
            services.AddInMemoryStreamDatabase();
            services.ConfigureStreamStore();
        }

        protected override async Task SetUp()
        {
            streamIdentifiers = GenerateStreamIdentifiers();
            store = new StreamStore(await SetupDatabase(), GetSerializer());
        }

        IEventSerializer GetSerializer()
        {
            return Services.GetRequiredService<IEventSerializer>();
        }

        async Task<IStreamDatabase> SetupDatabase()
        {
            var fixture = new Fixture();

            var database = Services.GetRequiredService<IStreamDatabase>();

            foreach (var streamId in streamIdentifiers!)
            {
                await database
                    .BeginAppendAsync(streamId)
                    .AddRangeAsync(fixture.CreateEventItems(100), CancellationToken.None)
                    .SaveChangesAsync(CancellationToken.None);
            }

            return database;
        }

        private static Id[] GenerateStreamIdentifiers()
        {
            return Enumerable.Range(0, 100).Select(i => GeneratedValues.Id).ToArray();
        }
    }
}
