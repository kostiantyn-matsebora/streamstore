//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
//using StreamStore.Storage;

//namespace StreamStore.Testing
//{
//    internal static class IStreamWriterExtension
//    {
//        public static async Task<IStreamWriter> AppendManyAsync(this IStreamWriter writer, IEnumerable<IEventRecord> records, CancellationToken token = default)
//        {
//            foreach (var record in records)
//            {
//                await writer.AppendAsync(record, token);
//            }
//            return writer;
//        }

//        public static async Task<IStreamWriter> AppendManyAsync(this Task<IStreamWriter> writer, IEnumerable<IEventRecord> records, CancellationToken token = default)
//        {
//            await FuncExtension.ThrowOriginalExceptionIfOccured(async() => await writer.GetAwaiter().GetResult().AppendManyAsync(records, token));
//            return writer.Result;
//        }

//        public static async Task<IStreamWriter> CommitAsync(this Task<IStreamWriter> writer, CancellationToken token = default)
//        {
//            await FuncExtension.ThrowOriginalExceptionIfOccured(async () => await writer.GetAwaiter().GetResult().ComitAsync(token));
//            return writer.Result;
//        }
//    }
//}
