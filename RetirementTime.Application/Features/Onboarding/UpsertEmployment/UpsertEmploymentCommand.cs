using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Onboarding.UpsertEmployment;

public record UpsertEmploymentCommand : IRequest<UpsertEmploymentResult>
{
    public required long UserId { get; init; }
    
    public required bool IsEmployed { get; init; }
    public required bool IsSelfEmployed { get; init; }
    public int? PlannedRetirementAge { get; init; }
    public int? CppContributionYears { get; init; }
    
    public required bool HasRoyalties { get; init; }
    public required bool HasDividends { get; init; }
    public required bool HasRentalIncome { get; init; }
    public required bool HasOtherIncome { get; init; }
}

public record UpsertEmploymentResult : BaseResult
{
    public long? EmploymentId { get; init; }
}
