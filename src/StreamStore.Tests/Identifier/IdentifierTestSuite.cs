using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.Tests.Identifier
{
    public class IdentifierTestSuite: TestSuiteBase
    {
        public Id CreateId(string? id = null)
        {
            return new Id(id ?? Generated.String);
        }
    }
}
