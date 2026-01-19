using MediatR;

namespace RetirementTime.Application.Features.Users.ValidateSession;

public record ValidateSessionQuery : IRequest<ValidateSessionResult>
{
    public required string SessionToken { get; init; }
}

public record ValidateSessionResult
{
    public bool IsValid { get; init; }
    public long? UserId { get; init; }
}

