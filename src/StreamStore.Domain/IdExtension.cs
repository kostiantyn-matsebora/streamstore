using System;

namespace StreamStore
{
    public static class IdExtension
    {
        public static Id ThrowIfHasNoValue(this Id id, string name) {
            if (id.Equals(Id.None)) throw new ArgumentNullException(nameof(id), $"{name} should have value.");
            return id;
        }
    }
}
