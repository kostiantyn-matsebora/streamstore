using System;

namespace StreamStore.Testing.Framework
{
    public interface ITestStorage: IDisposable
    {
        bool EnsureExists();
    }
}
