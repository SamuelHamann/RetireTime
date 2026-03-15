using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.UpsertMainResidence;

public record UpsertMainResidenceCommand : IRequest<UpsertMainResidenceResult>
{
    public required long UserId { get; init; }
    public required bool HasMainResidence { get; init; }
    public decimal? PurchasePrice { get; init; }
    public decimal? MonthlyMortgagePayments { get; init; }
    public decimal? MortgageLeft { get; init; }
    public decimal? YearlyInsurance { get; init; }
    public decimal? MonthlyElectricityCosts { get; init; }
    public int? MortgageDuration { get; init; }
    public DateTime? MortgageStartDate { get; init; }
    public decimal? EstimatedValue { get; init; }
}

public record UpsertMainResidenceResult : BaseResult
{
    public long? MainResidenceId { get; init; }
}

