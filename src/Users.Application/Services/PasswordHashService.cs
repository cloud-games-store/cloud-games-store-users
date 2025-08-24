using Users.Domain.Interfaces;

namespace Users.Application.Services;

public class PasswordHashService : IPasswordHash
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        throw new NotImplementedException();
    }
}
