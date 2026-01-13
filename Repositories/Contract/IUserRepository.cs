using Active_Blog_Service.Models;
using Active_Blog_Service_API.Repositories.Contract;

namespace Active_Blog_Service.Repositories.Contracts
{
    public interface IUserRepository : IScoppedRepositoryMarker
    {

        Task<User> GetUserByEmailAsync(string email);

        User GetUserByEmail(string email);
    }
}
