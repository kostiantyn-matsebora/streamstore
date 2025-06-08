using System;
using System.Collections.Generic;
using System.Text;
using StreamStore.Extensions;

namespace StreamStore.Configuration
{
    public class ComponentSpecificationBase: IComponentSpecification
    {
        readonly List<Type> requiredDependencies = new List<Type>();

        public Type[] RequiredDependencies => requiredDependencies.ToArray();

        protected ComponentSpecificationBase AddRequiredDependency<TDependency>()
        {
            return AddRequiredDependency(typeof(TDependency));
        }

        protected ComponentSpecificationBase AddRequiredDependency(Type dependencyType)
        {
            dependencyType.ThrowIfNull(nameof(dependencyType));
            requiredDependencies.Add(dependencyType);
            return this;
        }

    }
}
