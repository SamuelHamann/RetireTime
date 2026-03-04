using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.UpsertInvestmentProperties;

public class UpsertInvestmentPropertiesCommand : IRequest<UpsertInvestmentPropertiesResult>
{
    public long UserId { get; set; }
    public bool HasInvestmentProperties { get; set; }
    public List<InvestmentPropertyInputDto> Properties { get; set; } = new();
}

public class InvestmentPropertyInputDto
{
    public long? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal PurchasePrice { get; set; }
    public decimal MonthlyMortgagePayments { get; set; }
    public decimal MortgageLeft { get; set; }
    public decimal YearlyInsurance { get; set; }
    public decimal? MonthlyElectricityCosts { get; set; }
    public int MortgageDuration { get; set; }
    public DateTime MortgageStartDate { get; set; }
    public decimal? EstimatedValue { get; set; }
    public decimal MonthlyCost { get; set; }
    public decimal MonthlyRevenue { get; set; }
}

public record UpsertInvestmentPropertiesResult : BaseResult
{
    public List<long> PropertyIds { get; init; } = new();
}

