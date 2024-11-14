namespace StreamStore.S3.AWS
{
    public static class SingleTenantConfiguratorExtension
    {
        public static ISingleTenantConfigurator UseAWSDatabase(this ISingleTenantConfigurator configurator)
        {
            return configurator.UseDatabase<S3StreamDatabase>(services =>
            {
                new AWSS3DatabaseConfigurator(services).Configure();
            });
        }
    }
}
