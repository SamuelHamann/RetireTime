using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.PersistingIncome.UpdateOtherPersistingIncome;

public record UpdateOtherPersistingIncomeCommand : IRequest<UpdateOtherPersistingIncomeResult>
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal? Amount { get; init; }
    public int FrequencyId { get; init; }
    public bool Taxable { get; init; }
}

public record UpdateOtherPersistingIncomeResult : BaseResult;

