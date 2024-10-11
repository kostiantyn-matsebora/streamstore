using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.S3.AWS
{
    public static class ServiceCollectionExtension
    {

        public static AWSS3DatabaseConfigurator ConfigureS3AmazonStreamStoreDatabase(this IServiceCollection services)
        {
            return new AWSS3DatabaseConfigurator(services);
        }

        public static IServiceCollection UseS3AmazonStreamStoreDatabase(this IServiceCollection services)
        {
            return new AWSS3DatabaseConfigurator(services).Configure();
        }
    }
}
