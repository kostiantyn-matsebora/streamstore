using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.Testing.Framework
{
    public abstract class DatabaseTestSuiteBase: TestSuiteBase
    {
        public IStreamDatabase Database => Services.GetRequiredService<IStreamDatabase>();
    }
}
