using Microsoft.EntityFrameworkCore.Storage;
using Project.InfrastructureLayer.Abstractions;

namespace Project.InfrastructureLayer.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CompleteAsync();

        public Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
