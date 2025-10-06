using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;
using Users.Domain.Interfaces;
using Users.Infra.Data.Context;

namespace Users.Infra.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task DeleteUser(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetUser(Guid id)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<User?> GetUser(string email)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task UpdateUser(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UserEmailAlreadyExists(string email)
    {
        return await _context.Users.AnyAsync(user => user.Email == email);
    }
}
