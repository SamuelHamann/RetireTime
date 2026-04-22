using MediatR;
using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Application.Features.Dashboard.PersistingIncome.GetOtherPersistingIncomes;

public record GetOtherPersistingIncomesQuery(long ScenarioId) : IRequest<GetOtherPersistingIncomesResult>;

public record GetOtherPersistingIncomesResult
{
    public List<OtherPersistingIncomeDto> Items { get; init; } = [];
    public List<Frequency> Frequencies { get; init; } = [];
}

public record OtherPersistingIncomeDto
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal? Amount { get; init; }
    public int FrequencyId { get; init; }
    public bool Taxable { get; init; }
}
