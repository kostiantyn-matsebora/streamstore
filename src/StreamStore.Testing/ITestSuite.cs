
namespace StreamStore.Testing
{
    public interface ITestSuite
    {

        void Initialize();

        Task WithDatabase(Func<IStreamDatabase, Task> action);

        bool IsReady { get; }
    }
}