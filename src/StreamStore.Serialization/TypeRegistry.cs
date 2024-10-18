using System;
using System.Collections.Concurrent;


namespace StreamStore.Serialization
{
    public sealed class TypeRegistry : ITypeRegistry
    {
        readonly ConcurrentDictionary<string, Type> types = new ConcurrentDictionary<string, Type>();
        readonly ConcurrentDictionary<Type, string> names = new ConcurrentDictionary<Type, string>();
        private TypeRegistry()
        {
        }

        void Initialize()
        {
            // no-op
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    var name = ComposeName(type);
                    types.GetOrAdd(name, type);
                    names.GetOrAdd(type, name);
                }
            }
        }

        public string ResolveNameByType(Type type)
        {
            return names.GetOrAdd(type, _ => ComposeName(_));
        }

        public Type ResolveTypeByName(string name)
        {
            return types.GetOrAdd(name, _ => Type.GetType(_));
        }

        static string ComposeName(Type type)
        {
            return $"{type.FullName}, {type.Assembly.GetName().Name}";
        }

        public static TypeRegistry CreateAndInitialize()
        {
            TypeRegistry registry = new TypeRegistry();
            registry.Initialize();
            return registry;
        }
    }
}
