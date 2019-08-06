using System;
using System.Threading.Tasks;
using Course.Identity.Domain.Models;

namespace Course.Identity.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Guid id);
        Task<User> GetAsync(string email);
        Task AddAsync(User user);
    }
}