using System.Linq.Expressions;
using Users.Domain.Entities;

namespace Users.Domain.Interfaces;

public interface IUserRepository
{
    public Task CreateUser(User user);
    public Task UpdateUser(User user);
    public Task<bool> UserEmailAlreadyExists(string email);
    public Task<User> GetUser(Guid id);
}
