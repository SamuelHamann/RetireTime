using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.PersistingIncome.CreateOtherPersistingIncome;

public record CreateOtherPersistingIncomeCommand(long ScenarioId) : IRequest<CreateOtherPersistingIncomeResult>;

public record CreateOtherPersistingIncomeResult : BaseResult
{
    public long IncomeId { get; init; }
}

