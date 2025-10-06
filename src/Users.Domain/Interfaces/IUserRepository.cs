using System.Linq.Expressions;
using Users.Domain.Entities;

namespace Users.Domain.Interfaces;

public interface IUserRepository
{
    public Task<Guid> CreateUser(User user);
    public Task UpdateUser(User user);
    public Task DeleteUser(User user);
    public Task<bool> UserEmailAlreadyExists(string email);
    public Task<User?> GetUser(Guid id);
    public Task<User?> GetUser(string email);
}
