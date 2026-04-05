using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Onboarding.GetEmployment;

public record GetEmploymentQuery : IRequest<GetEmploymentResult>
{
    public required long UserId { get; init; }
}

public record GetEmploymentResult : BaseResult
{
    public EmploymentDto? Employment { get; init; }
}

public record EmploymentDto
{
    public long Id { get; init; }
    
    public bool IsEmployed { get; init; }
    public bool IsSelfEmployed { get; init; }
    public int? PlannedRetirementAge { get; init; }
    public int? CppContributionYears { get; init; }
    
    public bool HasRoyalties { get; init; }
    public bool HasDividends { get; init; }
    public bool HasRentalIncome { get; init; }
    public bool HasOtherIncome { get; init; }
}
