using LDCBackendProject.Core.Entities;
using LDCBackendProject.Core.Interfaces;
using LDCBackendProject.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDCBackendProject.BLL.UseCases
{
    public class UserService : IUserService
    {
        protected IUnitOfWork _unitOfWork { get; set; }
        public UserService(IUserRepository userRepository)
        {
            //_userRepository = userRepository;
        }

        User IUserService.GetUserById(int id)
        {
            throw new NotImplementedException();
        }
        //public User GetUserById(int id)
        //{
        //    //return _userRepository.GetById(id);
        //}
    }
}
