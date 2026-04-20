using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.UpdateDefinedProfitSharing;

public record UpdateDefinedProfitSharingCommand : IRequest<BaseResult>
{
    public required long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal? PercentOfSalaryEmployer { get; init; }
}
