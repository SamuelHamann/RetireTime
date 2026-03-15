using RetirementTime.Domain.Entities;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IUserProgressRepository
{
    Task<UserProgress?> GetByUserId(long userId);
    Task<UserProgress> Upsert(UserProgress userProgress);
}

