
namespace StreamStore.Testing
{
    public interface ITestSuite
    {
        void SetUpSuite();

        IServiceProvider Services { get; }
        bool ArePrerequisitiesMet { get; }

    }
}