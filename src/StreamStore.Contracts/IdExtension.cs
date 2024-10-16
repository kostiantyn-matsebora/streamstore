using System;

namespace StreamStore
{
    public static class IdExtension
    {
        public static bool HasValue(this Id id) => !id.Equals(Id.None);

        public static void ThrowIfHasNoValue(this Id id) {
            if (id.Equals(Id.None)) throw new ArgumentNullException(nameof(id), "Id should have value.");
        }
    }
}
