namespace StreamStore.S3.AWS
{
    public static class SingleTenantConfiguratorExtension
    {
        public static ISingleTenantDatabaseConfigurator UseAWSDatabase(this ISingleTenantDatabaseConfigurator configurator)
        {
            return configurator.UseDatabase<S3StreamDatabase>(services =>
            {
                new AWSS3DatabaseConfigurator(services).Configure();
            });
        }
    }
}
