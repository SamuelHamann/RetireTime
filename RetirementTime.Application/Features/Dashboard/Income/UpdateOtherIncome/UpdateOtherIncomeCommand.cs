using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.UpdateOtherIncome;

public record UpdateOtherIncomeCommand : IRequest<BaseResult>
{
    public required long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal? Gross { get; init; }
    public int GrossFrequencyId { get; init; }
    public decimal? Net { get; init; }
    public int NetFrequencyId { get; init; }
}
