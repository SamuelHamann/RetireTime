using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IOasConstantsRepository
{
    /// <summary>
    /// Returns the most recently added OAS constants row, representing the current
    /// government-published rates and thresholds.
    /// </summary>
    Task<OasConstants?> GetLatestAsync();
}

