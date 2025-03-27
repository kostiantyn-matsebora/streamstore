using Moq;

namespace StreamStore.Testing
{
    public static partial class Generated
    {
        public static class Mocks {
            public static Mock<T> Single<T>() where T : class
            {
                return new Mock<T>();
            }
        }
    }
}
