using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.SaveOasCppIncome;

public record SaveOasCppIncomeCommand : IRequest<BaseResult>
{
    public required long ScenarioId { get; init; }
    public required long TimelineId { get; init; }
    public decimal? IncomeLastYear { get; init; }
    public decimal? Income2YearsAgo { get; init; }
    public decimal? Income3YearsAgo { get; init; }
    public decimal? Income4YearsAgo { get; init; }
    public decimal? Income5YearsAgo { get; init; }
    public int? YearsSpentInCanada { get; init; }
}
