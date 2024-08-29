using Microsoft.EntityFrameworkCore.Storage;
using Project.InfrastructureLayer.Interfaces;

namespace Project.InfrastructureLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CompleteAsync();

        public Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
