﻿
//using Cassandra;

//namespace StreamStore.NoSql.Cassandra.Configuration
//{
//    internal class CassandraClusterBuilder
//    {

//        readonly Cluster cluster;

//        public CassandraClusterBuilder(CassandraClusterConfiguration configuration)
//        {
//            configuration.ThrowIfNull(nameof(configuration));
//            cluster = Cluster.Builder().AddContactPoints(configuration.ContactPoints).Build();
//        }

//        public Cluster Build()
//        {
//            return cluster;
//        }
//    }
//}