using System;
using System.Threading.Tasks;

namespace StreamStore.Testing.Framework
{
    public interface ITestStorage: IDisposable
    {
        bool EnsureExists();
    }
}
