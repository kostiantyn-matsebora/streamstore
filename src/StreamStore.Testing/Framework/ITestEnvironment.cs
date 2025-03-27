
using System;

namespace StreamStore.Testing
{
    public interface ITestEnvironment
    {
        void SetUp();

        IServiceProvider Services { get; }
        bool IsReady { get; }

    }
}