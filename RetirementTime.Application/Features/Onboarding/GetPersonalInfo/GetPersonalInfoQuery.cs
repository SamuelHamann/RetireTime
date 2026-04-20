using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Onboarding.GetPersonalInfo;

public record GetPersonalInfoQuery : IRequest<GetPersonalInfoResult>
{
    public required long UserId { get; init; }
}

public record GetPersonalInfoResult : BaseResult
{
    public PersonalInfoDto? PersonalInfo { get; init; }
}

public record PersonalInfoDto
{
    public long Id { get; init; }
    // These come from User table - populated by the handler
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    // These come from OnboardingPersonalInfo table
    public DateOnly DateOfBirth { get; init; }
    public required string CitizenshipStatus { get; init; }
    public required string MaritalStatus { get; init; }
    public bool HasCurrentChildren { get; init; }
    public bool PlansFutureChildren { get; init; }
    public bool IncludePartner { get; init; }
}
