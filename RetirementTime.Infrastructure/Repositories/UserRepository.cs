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
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}

