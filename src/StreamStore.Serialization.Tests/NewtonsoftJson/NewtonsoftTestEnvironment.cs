﻿using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing.Serializer;

namespace StreamStore.Serialization.Tests.NewtonsoftJson
{
    public partial class NewtonsoftTestEnvironment : SerializerTestEnvironmentBase
    {
        protected override IEventSerializer CreateSerializer(IServiceProvider services)
        {
            return new NewtonsoftEventSerializer(services.GetRequiredService<ITypeRegistry>(), false);
        }
    }
}
