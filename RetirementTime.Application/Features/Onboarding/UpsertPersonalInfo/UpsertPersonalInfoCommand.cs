using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Onboarding.UpsertPersonalInfo;

public record UpsertPersonalInfoCommand : IRequest<UpsertPersonalInfoResult>
{
    public required long UserId { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required string CitizenshipStatus { get; init; }
    public required string MaritalStatus { get; init; }
    public required bool HasCurrentChildren { get; init; }
    public required bool PlansFutureChildren { get; init; }
    public required bool IncludePartner { get; init; }
}

public record UpsertPersonalInfoResult : BaseResult
{
    public long? PersonalInfoId { get; init; }
}
