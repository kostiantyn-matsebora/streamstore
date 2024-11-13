namespace StreamStore.Serialization
{

    public static class StreamStoreConfiguratorExtension
    {
        public static ISerializationConfigurator UseTextJsonSerializer(this ISerializationConfigurator configurator)
        {
            configurator.UseSerializer<SystemTextJsonEventSerializer>();
            return configurator;
        }
    }
}
