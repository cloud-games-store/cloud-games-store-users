namespace Users.Domain.Interfaces;

public interface IPasswordHash
{
    public string Hash(string password);
    public bool VerifyPassword(string password, string hash);
}
