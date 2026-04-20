using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.UpdatePensionDefinedBenefits;

public record UpdatePensionDefinedBenefitsCommand : IRequest<BaseResult>
{
    public required long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int StartAge { get; init; }
    public decimal? BenefitsPre65 { get; init; }
    public decimal? BenefitsPost65 { get; init; }
    public int PercentIndexedToInflation { get; init; }
    public int PercentSurvivorBenefits { get; init; }
    public decimal? RrspAdjustment { get; init; }
}
