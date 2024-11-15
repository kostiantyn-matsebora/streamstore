namespace StreamStore.Serialization.Protobuf
{
    public static class StreamStoreConfiguratorExtension
    {
        public static ISerializationConfigurator WithProtobufSerializer(this ISerializationConfigurator configurator, bool compression = true)
        {
            return configurator.UseSerializer<ProtobufEventSerializer>();
        }
    }
}
