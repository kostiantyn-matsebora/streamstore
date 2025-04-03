using System;


namespace StreamStore.Testing
{
    public static partial class Generated
    {
        public static class Names
        {
            public static string Storage => "test_" + Guid.NewGuid().ToString().Replace("-", "_");
        }
    }
}
