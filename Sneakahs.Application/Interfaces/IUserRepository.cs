using Sneakahs.Domain.Entities;
using System.Threading.Tasks;

namespace Sneakahs.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(string email);
        Task AddUser(User user);
    }
}