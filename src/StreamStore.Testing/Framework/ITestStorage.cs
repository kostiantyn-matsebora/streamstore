using System;
using System.Threading.Tasks;

namespace StreamStore.Testing.Framework
{
    public interface ITestStorage: IDisposable
    {
        Task<bool> EnsureExistsAsync();
    }
}
