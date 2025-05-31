using Sneakahs.Domain.Entities;
using System.Threading.Tasks;

namespace Sneakahs.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(string email);
        Task<User?> FindById(Guid id);
        Task AddUser(User user);
    }
}