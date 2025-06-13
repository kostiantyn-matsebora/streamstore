using AutoFixture;

namespace StreamStore.Storage.EventFlow.Tests
{
    internal static class Generated
    {
        public static string StorageName => "test_" + Guid.NewGuid().ToString().Replace("-", "_");
        public static Id Id => new Fixture().Create<Id>();
        public static string String => new Fixture().Create<string>();
    }
}
