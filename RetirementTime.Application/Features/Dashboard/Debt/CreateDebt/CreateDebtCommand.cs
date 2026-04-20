using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Debt.CreateDebt;

public record CreateDebtCommand(long ScenarioId, long DebtTypeId, long? DebtAgainstAssetId = null) : IRequest<CreateDebtResult>;

public record CreateDebtResult : BaseResult
{
    public long DebtId { get; init; }
}
