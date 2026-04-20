using MediatR;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;

namespace RetirementTime.Application.Features.Dashboard.Asset.GetInvestmentAccounts;

public record GetInvestmentAccountsQuery(long ScenarioId) : IRequest<GetInvestmentAccountsResult>;

public record GetInvestmentAccountsResult
{
    public List<AssetsInvestmentAccount> Accounts { get; init; } = [];
    public List<AccountType> AccountTypes { get; init; } = [];
}
