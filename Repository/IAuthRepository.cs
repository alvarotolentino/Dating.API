using System.Threading.Tasks;
using Dating.API.Models;

namespace Dating.API.Repository
{
    public interface IAuthRepository: IGenericRepository<User>
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string username, string password);
         Task<bool> UserExists(string username);

    }
}