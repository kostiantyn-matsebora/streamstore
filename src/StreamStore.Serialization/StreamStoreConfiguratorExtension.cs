namespace StreamStore.Serialization
{

    public static class StreamStoreConfiguratorExtension
    {
        public static ISerializationConfigurator WithTextJsonSerializer(this ISerializationConfigurator configurator)
        {
            configurator.UseSerializer<SystemTextJsonEventSerializer>();
            return configurator;
        }
    }
}
