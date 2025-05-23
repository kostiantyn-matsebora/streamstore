
namespace StreamStore
{
    internal interface IStreamUnitOfWorkFactory
    {
        IStreamUnitOfWork Create(Id streamId, Revision expectedRevision);
    }
}
