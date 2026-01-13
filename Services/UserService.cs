using Active_Blog_Service.Models;
using Active_Blog_Service.Repositories.Contracts;
using Active_Blog_Service.Services.Contracts;
using System.Runtime.CompilerServices;

namespace Active_Blog_Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _applicationUserRepository;
        public UserService(IUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }
     
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _applicationUserRepository.GetUserByEmailAsync(email);
        }
       
        public bool CheckFoundOfEmail(string email)
        {
            var user = _applicationUserRepository.GetUserByEmail(email);
            if (user != null)
                return true;
            return false;
        }
    }
}
