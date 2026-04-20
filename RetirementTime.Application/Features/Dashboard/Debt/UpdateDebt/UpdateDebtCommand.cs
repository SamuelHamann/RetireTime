using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Debt.UpdateDebt;

public record UpdateDebtCommand : IRequest<BaseResult>
{
    public required long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public long DebtTypeId { get; init; }
    public decimal? Balance { get; init; }
    public decimal? InterestRate { get; init; }
    public int FrequencyId { get; init; }
    public int? TermInYears { get; init; }
    public long? DebtAgainstAssetId { get; init; }
}
