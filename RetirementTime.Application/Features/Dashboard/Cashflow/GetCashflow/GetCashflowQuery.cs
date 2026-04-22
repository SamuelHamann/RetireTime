using MediatR;

namespace RetirementTime.Application.Features.Dashboard.Cashflow.GetCashflow;

public record GetCashflowQuery : IRequest<GetCashflowResult>
{
    public required long ScenarioId { get; init; }

    // Localized category labels passed in from the presentation layer
    public required string Label_Employment { get; init; }
    public required string Label_SelfEmployment { get; init; }
    public required string Label_DefinedBenefits { get; init; }
    public required string Label_OtherIncome { get; init; }
    public required string Label_TotalIncome { get; init; }
    public required string Label_LivingExpenses { get; init; }
    public required string Label_DiscretionaryExpenses { get; init; }
    public required string Label_DebtRepayments { get; init; }
    public required string Label_AssetsExpenses { get; init; }
    public required string Label_OtherExpenses { get; init; }
    public required string Label_TotalExpenses { get; init; }
    public required string Label_Savings { get; init; }
}

