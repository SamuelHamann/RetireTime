using RetirementTime.Domain.Entities.BeginnerGuide.Benefits;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IBeginnerGuidePensionTypeRepository
{
    Task<List<BeginnerGuidePensionType>> GetAllAsync();
}
