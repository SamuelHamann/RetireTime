using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Users.Login;

public record LoginCommand : IRequest<LoginResult>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public record LoginResult : BaseResult
{
    public long? UserId { get; init; }
    public string? FirstName { get; init; }
    public int? RoleId { get; init; }
    public string? RoleName { get; init; }
}

