using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.PersistingIncome.UpdateRealEstateIncome;

public record UpdateRealEstateIncomeCommand : IRequest<UpdateRealEstateIncomeResult>
{
    public long Id { get; init; }
    public long? InvestmentPropertyId { get; init; }
    public string PropertyName { get; init; } = string.Empty;
    public decimal? Amount { get; init; }
    public int? FrequencyId { get; init; }
}

public record UpdateRealEstateIncomeResult : BaseResult;

