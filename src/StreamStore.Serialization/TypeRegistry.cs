using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace StreamStore.Serialization
{
    public sealed class TypeRegistry : ITypeRegistry
    {
        readonly ConcurrentDictionary<string, Type> types = new ConcurrentDictionary<string, Type>();
        readonly ConcurrentDictionary<Type, string> names = new ConcurrentDictionary<Type, string>();
        private TypeRegistry()
        {
        }
        public static TypeRegistry CreateAndInitialize()
        {
            TypeRegistry registry = new TypeRegistry();
            registry.Initialize();
            return registry;
        }

        public string ResolveNameByType(Type type)
        {
            return names.GetOrAdd(type, _ => ComposeName(_));
        }

        public Type ResolveTypeByName(string name)
        {
            return types.GetOrAdd(name, _ => Type.GetType(_));
        }

        void Initialize()
        {
            // no-op
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                foreach (var type in GetLoadableTypes(assembly))
                {
                    var name = ComposeName(type);
                    types.GetOrAdd(name, type);
                    names.GetOrAdd(type, name);
                }
            }
        }

        static string ComposeName(Type type)
        {
            return $"{type.FullName}, {type.Assembly.GetName().Name}";
        }

        static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}
