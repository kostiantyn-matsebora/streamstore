using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.Testing.Framework
{
    public interface ITestSuite
    {
        Task SetUpSuite();

        IServiceProvider Services { get; }
        bool ArePrerequisitiesMet { get; }

    }
}