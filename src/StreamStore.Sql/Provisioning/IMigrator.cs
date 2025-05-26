using System;
namespace StreamStore.Sql.Provisioning
{
    public interface IMigrator
    {
        void Migrate();
    }
}
