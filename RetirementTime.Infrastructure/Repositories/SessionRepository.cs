using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class SessionRepository(ApplicationDbContext context) : ISessionRepository
{
    public async Task<Session> CreateSession(Session session)
    {
        context.Sessions.Add(session);
        await context.SaveChangesAsync();
        return session;
    }

    public async Task<Session?> GetSessionByToken(string sessionToken)
    {
        if (!Guid.TryParse(sessionToken, out Guid sessionId))
            return null;

        return await context.Sessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.SessionId == sessionId && s.ValidUntil > DateTime.UtcNow);
    }

    public async Task<bool> ValidateSession(string sessionToken)
    {
        if (!Guid.TryParse(sessionToken, out Guid sessionId))
            return false;

        return await context.Sessions
            .AnyAsync(s => s.SessionId == sessionId && s.ValidUntil > DateTime.UtcNow);
    }

    public async Task<bool> RefreshSession(string sessionToken)
    {
        if (!Guid.TryParse(sessionToken, out Guid sessionId))
            return false;

        var session = await context.Sessions
            .FirstOrDefaultAsync(s => s.SessionId == sessionId && s.ValidUntil > DateTime.UtcNow);

        if (session == null)
            return false;

        session.ValidUntil = DateTime.UtcNow.AddMinutes(30);
        await context.SaveChangesAsync();
        return true;
    }
}

