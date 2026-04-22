using MediatR;

namespace RetirementTime.Application.Features.Dashboard.PersistingIncome.GetRealEstateIncomes;

public record GetRealEstateIncomesQuery(long ScenarioId) : IRequest<List<RealEstateIncomeDto>>;

public record RealEstateIncomeDto
{
    public long Id { get; init; }
    public long? InvestmentPropertyId { get; init; }
    public string PropertyName { get; init; } = string.Empty;
    public decimal? Amount { get; init; }
    public int? FrequencyId { get; init; }
}

