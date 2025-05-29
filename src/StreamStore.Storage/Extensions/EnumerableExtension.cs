using System.Collections.Generic;
using System.Linq;

namespace StreamStore.Extensions
{
    public static class EnumerableExtension
    {
        public static string CommaSeparated<T>(this IEnumerable<T> enumerable)
        {
            return string.Join(", ", enumerable.Select(x => x!.ToString()));
        }

        public static bool NotNullAndNotEmpty<T>(this IEnumerable<T>? enumerable)
        {
           return enumerable != null && enumerable.Any();
        }

        public static bool NullOrEmpty<T>(this IEnumerable<T>? enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}
