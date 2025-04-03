namespace StreamStore.S3.AWS
{
    public static class SingleTenantConfiguratorExtension
    {
        public static ISingleTenantConfigurator UseAWSStorage(this ISingleTenantConfigurator configurator)
        {
            return configurator.UseStorage<S3StreamStorage>(services =>
            {
                new AWSS3StorageConfigurator(services).Configure();
            });
        }
    }
}
