
namespace StreamStore.Testing
{
    public interface ITestSuite
    {
        Task SetUpSuite();

        IServiceProvider Services { get; }
        bool ArePrerequisitiesMet { get; }

    }
}