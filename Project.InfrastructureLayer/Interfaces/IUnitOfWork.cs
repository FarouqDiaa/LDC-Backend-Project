using Project.InfrastructureLayer.Interfaces;

namespace Project.InfrastructureLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICustomerRepository Customers { get; }
        IOrderRepository Orders { get; }
        IOrderItemRepository OrderItems { get; }
        Task<int> CompleteAsync();


    }
}
