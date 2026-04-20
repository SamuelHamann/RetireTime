using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User> CreateUser(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserById(long userId)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<bool> UpdateHasCompletedIntro(long userId, bool hasCompletedIntro)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return false;

        user.HasCompletedIntro = hasCompletedIntro;
        await context.SaveChangesAsync();
        return true;
    }
}

