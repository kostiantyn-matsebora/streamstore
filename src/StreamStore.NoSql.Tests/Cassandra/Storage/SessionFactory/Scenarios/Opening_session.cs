using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Storage.SessionFactory
{
    public class Opening_session: Scenario<SessionFactoryTestEnvironment>
    {
        [Fact]
        public void When_session_is_opened()
        {
            // Act
            Environment.SessionFactory.Open();
            Environment.SessionFactory.Open(); // Make sure session opens only once

            // Assert
            Environment.MockRepository.VerifyAll();
        }


        [Fact]
        public void When_session_is_disposed()
        {
            // Arrange 
            Environment.Session.Setup(s => s.Dispose());

            // Act
            Environment.SessionFactory.Open();
            Environment.SessionFactory.Open(); // Make sure session opens only once
            Environment.SessionFactory.Dispose();

            // Assert
            Environment.MockRepository.VerifyAll();
        }
    }
}
