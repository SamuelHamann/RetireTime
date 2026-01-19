using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Users.CreateUser;

public record CreateUserCommand : IRequest<CreateUserResult>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required int CountryId { get; init; }
    public required int SubdivisionId { get; init; }
}

public record CreateUserResult : BaseResult
{
    public long? UserId { get; init; }
}

