using Sneakahs.Domain.Entities;

namespace Sneakahs.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(string email);
        Task<User?> FindById(Guid id);
        Task AddUser(User user);
    }
}