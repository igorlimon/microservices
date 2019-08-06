using System.Threading.Tasks;
using Course.Common.Auth;

namespace Course.Identity.Services
{
    public interface IUserService
    {
         Task RegisterAsync(string email, string password, string name);
         Task<JsonWebToken> LoginAsync(string email, string password);
    }
}