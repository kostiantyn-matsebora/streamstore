using System;


namespace StreamStore.Testing
{
    public static partial class Generated
    {
        public static class Names
        {
            public static string Database => "test_" + Guid.NewGuid().ToString().Replace("-", "_");
        }
    }
}
