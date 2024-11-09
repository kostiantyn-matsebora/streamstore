
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.Sql.Configuration
{

    public interface IConfigurator
    {
        IServiceCollection Configure();
        IServiceCollection Configure(IConfiguration configuration, string sectionName);
    }
}
