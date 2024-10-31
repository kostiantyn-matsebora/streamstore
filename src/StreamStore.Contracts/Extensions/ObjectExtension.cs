using System;

namespace StreamStore
{
    public static class ObjectExtension
    {

        public static T ThrowIfNull<T>(this T obj, string name)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(name);
            }

            return obj;
        }
    }
}
