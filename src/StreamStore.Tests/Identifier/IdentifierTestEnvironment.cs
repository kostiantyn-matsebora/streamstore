using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.Tests.Identifier
{
    public class IdentifierTestEnvironment: TestEnvironmentBase
    {
        public static Id CreateId(string? id = null)
        {
            return new Id(id ?? Generated.Primitives.String);
        }
    }
}
