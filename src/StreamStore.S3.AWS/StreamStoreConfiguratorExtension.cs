namespace StreamStore.S3.AWS
{
    public static class StreamStoreConfiguratorExtension
    {
        public static IStreamStoreConfigurator UseAWSDatabase(this IStreamStoreConfigurator configurator)
        {
            configurator.WithSingleTenant(registrator =>
            {
                registrator.UseDatabase<S3StreamDatabase>(services =>
                {
                    new AWSS3DatabaseConfigurator(services).Configure();
                });
            });

            return configurator;
        }
    }
}
