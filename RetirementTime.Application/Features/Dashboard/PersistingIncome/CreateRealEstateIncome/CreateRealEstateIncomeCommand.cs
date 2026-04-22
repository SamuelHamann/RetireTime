using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.PersistingIncome.CreateRealEstateIncome;

public record CreateRealEstateIncomeCommand(long ScenarioId, long? InvestmentPropertyId = null, string PropertyName = "") : IRequest<CreateRealEstateIncomeResult>;

public record CreateRealEstateIncomeResult : BaseResult
{
    public long IncomeId { get; init; }
}
