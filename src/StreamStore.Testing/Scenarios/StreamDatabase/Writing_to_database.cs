using FluentAssertions;

namespace StreamStore.Testing.Scenarios.StreamDatabase
{
    public abstract class Writing_to_database<TSuite> : DatabaseScenario<TSuite> where TSuite : DatabaseSuiteBase
    {
        protected Writing_to_database(TSuite suite) : base(suite)
        {
        }

        [Fact]
        public async Task When_parameters_are_absent_or_incorrect()
        {
            // Arrange
            var eventId = Id.None;
            var stream = Container.GetExistingStream();

            var uow = await Database.BeginAppendAsync(stream.Id, stream.Length);

            // Act
            var act = async() => await uow.AddAsync(eventId, GeneratedValues.DateTime, GeneratedValues.ByteArray);

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();

            // Act
            act = async () => await uow.AddAsync(GeneratedValues.Id, DateTime.MinValue, GeneratedValues.ByteArray);

            //Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();

            // Act
            act = async () => await uow.AddAsync(GeneratedValues.Id, GeneratedValues.DateTime, null!);

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
