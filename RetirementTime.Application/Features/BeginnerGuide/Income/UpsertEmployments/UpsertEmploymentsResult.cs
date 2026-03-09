using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.BeginnerGuide.Income.UpsertEmployments;

public record UpsertEmploymentsResult : BaseResult
{
    public long UserId { get; init; }
    public int EmploymentCount { get; init; }
}
