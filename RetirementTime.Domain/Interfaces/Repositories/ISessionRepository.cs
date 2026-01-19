using RetirementTime.Domain.Entities;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface ISessionRepository
{
    Task<Session> CreateSession(Session session);
    Task<Session?> GetSessionByToken(string sessionToken);
    Task<bool> ValidateSession(string sessionToken);
    Task<bool> RefreshSession(string sessionToken);
}

