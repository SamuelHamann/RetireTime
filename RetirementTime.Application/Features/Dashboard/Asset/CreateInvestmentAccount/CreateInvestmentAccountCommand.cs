using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Asset.CreateInvestmentAccount;

public record CreateInvestmentAccountCommand(long ScenarioId) : IRequest<CreateInvestmentAccountResult>;

public record CreateInvestmentAccountResult : BaseResult
{
    public long AccountId { get; init; }
}
