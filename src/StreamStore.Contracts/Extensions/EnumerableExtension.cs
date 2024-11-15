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
    }
}
