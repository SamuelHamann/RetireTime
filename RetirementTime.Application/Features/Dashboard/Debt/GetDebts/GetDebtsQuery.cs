using MediatR;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Debt;

namespace RetirementTime.Application.Features.Dashboard.Debt.GetDebts;

public record GetDebtsQuery(long ScenarioId, long[] DebtTypeIds) : IRequest<GetDebtsResult>;

public record GetDebtsResult
{
    public List<GenericDebt> Debts { get; init; } = [];
    public List<Frequency> Frequencies { get; init; } = [];
    public List<DebtType> DebtTypes { get; init; } = [];
}
