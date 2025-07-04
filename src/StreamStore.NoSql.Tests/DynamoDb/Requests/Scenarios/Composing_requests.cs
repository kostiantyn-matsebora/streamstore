using FluentAssertions;
using StreamStore.NoSql.DynamoDb;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.DynamoDb.Requests
{
    public class Composing_requests: Scenario
    {
        [Fact]
        public void When_any_argument_is_not_set()
        {
            // Act
            var act = () => new DynamoDbRequests(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_composing_getting_stream_metadata_request()
        {

            // Arrange
            var config = Generated.Objects.Single<DynamoDbConfiguration>();

            var requests = new DynamoDbRequests(config);

            var streamId = Generated.Primitives.String;

            // Act
            var request = requests.GetMetadata(streamId);

            // Assert
            request.Should().NotBeNull();
            request.TableName.Should().Be(config.TableName);
            request.KeyConditionExpression.Should().Contain(":streamId");
            request.KeyConditionExpression.Should().Contain(AttributeNames.StreamId);
            request.ExpressionAttributeValues.Keys.Should().Contain(":streamId");
            request.ExpressionAttributeValues.Values.Should().Satisfy(v => v.S == streamId);
        }

        [Fact]
        public void When_composing_deleting_stream_revisions()
        {

            // Arrange
            var config = Generated.Objects.Single<DynamoDbConfiguration>();

            var requests = new DynamoDbRequests(config);

            var streamId = Generated.Primitives.String;

            var revisions = Generated.Objects.Many<int>(10).ToArray();


            // Act
            var request = requests.DeleteStreamRevisions(streamId, revisions);

            // Assert
            request.Should().NotBeNull();
            request.RequestItems.Count.Should().Be(1);
            request.RequestItems.Keys.Should().Contain(config.TableName);
            request.RequestItems.Values.First().Should().NotBeNull();
            request.RequestItems.Values.First().Count.Should().Be(10);
            request.RequestItems.Values.First().Should().AllSatisfy(r => r.DeleteRequest.Should().NotBeNull());
            request.RequestItems.Values.First().Should().AllSatisfy(r => r.DeleteRequest.Key.Keys.Should().Contain(AttributeNames.StreamId));
            request.RequestItems.Values.First().Should().AllSatisfy(r => r.DeleteRequest.Key.Keys.Should().Contain(AttributeNames.Revision));
        }


        [Fact]
        public void When_composing_gettings_stream_events()
        {

            // Arrange
            var config = Generated.Objects.Single<DynamoDbConfiguration>();

            var requests = new DynamoDbRequests(config);

            var streamId = Generated.Primitives.String;

            var startFrom = 5;
            var count = 7;


            // Act
            var request = requests.GetStreamEvents(streamId, startFrom, count);


            // Assert
            request.Should().NotBeNull();
            request.TableName.Should().Be(config.TableName);
            request.Limit.Should().Be(count);
            request.KeyConditionExpression.Should().Contain(":streamId");
            request.KeyConditionExpression.Should().Contain(AttributeNames.StreamId);
            request.KeyConditionExpression.Should().Contain(":startFrom");
            request.KeyConditionExpression.Should().Contain(AttributeNames.Revision);
            request.ExpressionAttributeValues.Keys.Should().Contain(":streamId");
            request.ExpressionAttributeValues.Keys.Should().Contain(":startFrom");
            request.ExpressionAttributeValues[":streamId"].S.Should().Be(streamId);
            request.ExpressionAttributeValues[":startFrom"].N.Should().Be(startFrom.ToString());
            
        }
    }
}
