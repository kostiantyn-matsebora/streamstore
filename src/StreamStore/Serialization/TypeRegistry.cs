using System;
using System.Collections.Concurrent;


namespace StreamStore.Serialization
{
    class TypeRegistry
    {
        public static readonly TypeRegistry Instance = new TypeRegistry();

        ConcurrentDictionary<string, Type> types = new ConcurrentDictionary<string, Type>();

        public string ByType(Type type)
        {
            var name = ComposeName(type);

            types.GetOrAdd(ComposeName(type), type);

            return name;
        }

        public Type ByName(string name)
        {
            return types.GetOrAdd(name, _ => Type.GetType(name));
        }

        string ComposeName(Type type)
        {
            return $"{type.FullName}, {type.Assembly.GetName().Name}";
        }
    }
}
