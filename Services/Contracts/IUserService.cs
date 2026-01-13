using Active_Blog_Service.Models;
using Active_Blog_Service_API.Services.Contracts;

namespace Active_Blog_Service.Services.Contracts
{
    public interface IUserService : IScopedServiceMarker
    {
       
        Task<User> GetUserByEmailAsync(string email);
      
        bool CheckFoundOfEmail(string email);
    }
}
