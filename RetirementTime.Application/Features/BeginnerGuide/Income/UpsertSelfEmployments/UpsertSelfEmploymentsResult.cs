using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.BeginnerGuide.Income.UpsertSelfEmployments;

public record UpsertSelfEmploymentsResult : BaseResult
{
    public long UserId { get; init; }
    public int SelfEmploymentCount { get; init; }
}
