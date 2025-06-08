using System;


namespace StreamStore.Configuration
{
    public interface IComponentSpecification
    {
        public Type[] RequiredDependencies { get; }
    }
}
