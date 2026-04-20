using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IFrequencyRepository
{
    Task<List<Frequency>> GetFrequencies();
    Task<Frequency?> GetById(int id);
}

