using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.BeginnerGuide.Benefits.UpsertGovernmentPension;

public class UpsertGovernmentPensionCommand : IRequest<UpsertGovernmentPensionResult>
{
    public long UserId { get; set; }
    public int YearsWorked { get; set; }
    public bool HasSpecializedPublicSectorPension { get; set; }
    public string? SpecializedPensionName { get; set; }
}

public record UpsertGovernmentPensionResult : BaseResult
{
    public long Id { get; init; }
}

