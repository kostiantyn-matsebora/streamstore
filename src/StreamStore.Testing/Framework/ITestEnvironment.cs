
using System;
using System.Threading.Tasks;

namespace StreamStore.Testing
{
    public interface ITestEnvironment
    {
        Task SetUp();

        IServiceProvider Services { get; }
        bool IsReady { get; }

    }
}