//using System.Collections.Generic;
//using StreamStore.NoSql.Cassandra.Configuration;

//namespace StreamStore.NoSql.Cassandra
//{
//    class CassandraProvisioningStatementProvider
//    {
//        readonly CassandraKeyspaceConfiguration config;

//        public CassandraProvisioningStatementProvider(CassandraKeyspaceConfiguration config)
//        {
//            this.config = config.ThrowIfNull(nameof(config));
//        }

//        public IEnumerable<string> GetProvisioningQueries()
//        {
//            yield return 
//                 @$"CREATE TABLE IF NOT EXISTS {config.EventsTableName}
//                    (id text,
//                    stream_id text,
//                    revision int,
//                    timestamp timeuuid,
//                    data text,
//                    PRIMARY KEY(stream_id, revision)
//                    );";

//            yield return 
//                 @$"CREATE TABLE IF NOT EXISTS {config.EventPerStreamTableName}
//                   (id text,
//                   stream_id text,
//                   PRIMARY KEY((id, stream_id))
//                    )";

//            yield return
//               @$"
//                   CREATE TABLE IF NOT EXISTS {config.RevisionPerStreamTableName}
//                   (revision int,
//                   stream_id text,
//                   PRIMARY KEY((revision, stream_id))
//                   );";
//        }
//    }
//}
