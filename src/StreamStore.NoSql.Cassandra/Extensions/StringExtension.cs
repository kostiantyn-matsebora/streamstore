using System;

namespace StreamStore
{
    public static class StringExtension
    {
        public static string ThrowIfNullOrEmpty(this string? value, string name)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(name);
            }

            return value;
        }
    }
}
