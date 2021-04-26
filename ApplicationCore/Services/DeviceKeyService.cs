using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;

namespace ApplicationCore.Services
{
    public class UserService : IUserService
    {

        IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GetUser(string userName)
        {
            
            var userSpec = new UserWithDeviceKeySpecification(userName);
            return await _unitOfWork.UserRepository.FirstOrDefaultAsync(userSpec);
        }
    }
}