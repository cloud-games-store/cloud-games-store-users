using Users.Domain.Entities;

namespace Users.Domain.Interfaces;

public interface IUserRepository
{
    public Task CreateUser(User user);
    public Task<bool> UserEmailAlreadyExists(string email);
}
