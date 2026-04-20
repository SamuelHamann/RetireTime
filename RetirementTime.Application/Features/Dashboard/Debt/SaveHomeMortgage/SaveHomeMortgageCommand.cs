using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Debt.SaveHomeMortgage;

public record SaveHomeMortgageCommand : IRequest<BaseResult>
{
    public required long ScenarioId { get; init; }
    public decimal? Balance { get; init; }
    public decimal? InterestRate { get; init; }
    public int FrequencyId { get; init; }
    public int? TermInYears { get; init; }
    public long? DebtAgainstAssetId { get; init; }
}
