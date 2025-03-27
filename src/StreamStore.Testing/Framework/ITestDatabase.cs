using System;

namespace StreamStore.Testing.Framework
{
    public interface ITestDatabase: IDisposable
    {
        bool EnsureExists();
    }
}
