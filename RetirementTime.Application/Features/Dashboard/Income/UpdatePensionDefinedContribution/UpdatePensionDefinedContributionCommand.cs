using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.UpdatePensionDefinedContribution;

public record UpdatePensionDefinedContributionCommand : IRequest<BaseResult>
{
    public required long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal? PercentOfSalaryEmployee { get; init; }
    public decimal? PercentOfSalaryEmployer { get; init; }
}
