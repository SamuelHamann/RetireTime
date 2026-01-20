using RetirementTime.Domain.Entities;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> CreateUser(User user);
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserById(long userId);
}

