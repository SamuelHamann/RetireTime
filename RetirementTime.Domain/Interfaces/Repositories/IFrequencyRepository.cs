using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IFrequencyRepository
{
    Task<List<Frequency>> GetAllAsync();
    Task<Frequency?> GetByIdAsync(int id);
}
