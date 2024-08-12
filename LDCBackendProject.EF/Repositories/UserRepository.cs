using LDCBackendProject.Core.Entities;
using LDCBackendProject.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDCBackendProject.EF.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected ApplicationDbContext _context { get; set; }
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User GetById(int id) => _context.Set<User>().Find(id);
    }
}
