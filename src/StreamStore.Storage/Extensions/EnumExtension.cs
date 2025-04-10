using System;

namespace StreamStore.Extensions
{
    public static class EnumExtension
    {
        public static string ToLowerString<T>(this T value) where T : Enum
        {
            return Enum.GetName(typeof(T), value).ToLower();
        }

        public static T ToEnum<T>(this string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
