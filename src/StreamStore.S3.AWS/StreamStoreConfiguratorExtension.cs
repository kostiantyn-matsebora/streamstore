namespace StreamStore.S3.AWS
{
    public static class StreamStoreConfiguratorExtension
    {
        public static IStreamStoreConfigurator UseAWSDatabase(this IStreamStoreConfigurator configurator)
        {
            configurator.WithSingleDatabase(registrator =>
            {
                registrator.ConfigureWith(services =>
                {
                    new AWSS3DatabaseConfigurator(services).Configure();
                });
            });

            return configurator;
        }
    }
}
