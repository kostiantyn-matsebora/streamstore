using System.Diagnostics.CodeAnalysis;

namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    public static class Tenants
    {
        public const string Default = "streamstore";
        public const string Tenant1 = "tenant_1";
        public const string Tenant2 = "tenant_2";
        public const string Tenant3 = "tenant_3";
    }
}
