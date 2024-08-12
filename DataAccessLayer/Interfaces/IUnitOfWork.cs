using LDCBackendProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDCBackendProject.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IUserRepository Users { get; }
        IOrderRepository Orders { get; }
        ICategoryRepository Categories { get; }


    }
}
