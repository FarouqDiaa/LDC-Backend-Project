using Project.InfrastructureLayer.Interfaces;

namespace Project.InfrastructureLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CompleteAsync();


    }
}
