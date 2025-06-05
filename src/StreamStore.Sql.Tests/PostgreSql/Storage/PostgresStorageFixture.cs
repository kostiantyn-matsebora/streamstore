//using Npgsql;
//using StreamStore.Sql.PostgreSql;
//using StreamStore.Sql.Tests.Storage;
//using StreamStore.Testing;


//namespace StreamStore.Sql.Tests.PostgreSql.Storage
//{
//    public sealed class PostgresStorageFixture : SqlStorageFixtureBase<PostgresTestStorage>
//    {

//        public PostgresStorageFixture() : base(new PostgresTestStorage(Generated.Names.Storage))
//        {
//        }
       
//        public override void ConfigurePersistence(ISingleTenantConfigurator configurator)
//        {
//             configurator.UsePostgresStorage(
//                    c => c.ConfigurePersistence(
//                        x => x.WithConnectionString(testStorage.ConnectionString)));
//        }
//    }
//}
