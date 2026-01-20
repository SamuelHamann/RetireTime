using MediatR;

namespace RetirementTime.Application.Features.Users.GetUserById;

public record GetUserByIdQuery : IRequest<GetUserByIdResult?>
{
    public required long UserId { get; init; }
}

public record GetUserByIdResult
{
    public long Id { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string LanguageCode { get; init; }
}

