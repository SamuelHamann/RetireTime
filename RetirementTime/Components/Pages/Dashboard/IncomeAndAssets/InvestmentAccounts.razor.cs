using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Asset.CreateHolding;
using RetirementTime.Application.Features.Dashboard.Asset.CreateInvestmentAccount;
using RetirementTime.Application.Features.Dashboard.Asset.DeleteHolding;
using RetirementTime.Application.Features.Dashboard.Asset.DeleteInvestmentAccount;
using RetirementTime.Application.Features.Dashboard.Asset.GetInvestmentAccounts;
using RetirementTime.Application.Features.Dashboard.Asset.UpdateHolding;
using RetirementTime.Application.Features.Dashboard.Asset.UpdateInvestmentAccount;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Models.Asset;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class InvestmentAccounts : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private AssetNavigationService AssetNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private bool _isLoading = true;
    private List<AssetsInvestmentAccountItemModel> _accountItems = [];
    private List<AccountType> _accountTypes = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var result = await Mediator.Send(new GetInvestmentAccountsQuery(ScenarioId));
        _accountTypes = result.AccountTypes;
        _accountItems = result.Accounts.Select(a => new AssetsInvestmentAccountItemModel
        {
            Id = a.Id,
            AccountName = a.AccountName,
            AccountTypeId = a.AccountTypeId,
            AdjustedCostBasis = a.AdjustedCostBasis,
            CurrentTotalValue = a.CurrentTotalValue,
            UseIndividualHoldings = a.UseIndividualHoldings,
            Holdings = a.Holdings.Select(h => new AssetsHoldingItemModel
            {
                Id = h.Id,
                AssetName = h.AssetName,
                IsPubliclyTraded = h.IsPubliclyTraded,
                CurrentValue = h.CurrentValue,
                TickerSymbol = h.TickerSymbol,
                AdjustedCostBasis = h.AdjustedCostBasis
            }).ToList()
        }).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task AddAccount()
    {
        var result = await Mediator.Send(new CreateInvestmentAccountCommand(ScenarioId));
        if (result.Success)
            _accountItems.Add(new AssetsInvestmentAccountItemModel
            {
                Id = result.AccountId,
                AccountTypeId = (long)InvestmentAccountType.RRSP
            });
    }

    private async Task RemoveAccount(AssetsInvestmentAccountItemModel account)
    {
        await Mediator.Send(new DeleteInvestmentAccountCommand(account.Id));
        _accountItems.Remove(account);
    }

    private async Task SaveAccount(AssetsInvestmentAccountItemModel account)
    {
        if (account.Id == 0) return;
        await Mediator.Send(new UpdateInvestmentAccountCommand
        {
            Id = account.Id,
            AccountName = account.AccountName,
            AccountTypeId = account.AccountTypeId,
            AdjustedCostBasis = account.AdjustedCostBasis,
            CurrentTotalValue = account.CurrentTotalValue,
            UseIndividualHoldings = account.UseIndividualHoldings
        });
    }

    private async Task AddHolding(AssetsInvestmentAccountItemModel account)
    {
        var result = await Mediator.Send(new CreateHoldingCommand(account.Id));
        if (result.Success) account.Holdings.Add(new AssetsHoldingItemModel { Id = result.HoldingId });
    }

    private async Task RemoveHolding(AssetsInvestmentAccountItemModel account, AssetsHoldingItemModel holding)
    {
        await Mediator.Send(new DeleteHoldingCommand(holding.Id));
        account.Holdings.Remove(holding);
    }

    private async Task SaveHolding(AssetsHoldingItemModel holding)
    {
        if (holding.Id == 0) return;
        await Mediator.Send(new UpdateHoldingCommand
        {
            Id = holding.Id,
            AssetName = holding.AssetName,
            IsPubliclyTraded = holding.IsPubliclyTraded,
            CurrentValue = holding.CurrentValue,
            TickerSymbol = holding.TickerSymbol,
            AdjustedCostBasis = holding.AdjustedCostBasis
        });
    }

    private void NavigateNext() => AssetNavService.NavigateNext();
}
