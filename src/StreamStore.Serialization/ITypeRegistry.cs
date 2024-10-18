using System;

namespace StreamStore.Serialization
{
    public interface ITypeRegistry
    {
        Type ResolveTypeByName(string name);
        string ResolveNameByType(Type type);
    }
}