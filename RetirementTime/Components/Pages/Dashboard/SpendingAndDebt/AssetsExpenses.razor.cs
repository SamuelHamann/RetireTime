using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Spending.GetAssetsExpenses;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Models.Spending;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.SpendingAndDebt;

public partial class AssetsExpenses : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private SpendingNavigationService SpendingNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private bool _isLoading = true;
    private List<SpendingAssetsExpenseItemModel> _linkedItems = [];
    private List<SpendingAssetsExpenseItemModel> _standaloneItems = [];
    private List<Frequency> _frequencies = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var result = await Mediator.Send(new GetAssetsExpensesQuery(ScenarioId));
        _frequencies = result.Frequencies;
        var existing = result.Expenses;

        // ── Home ──────────────────────────────────────────────────────────────
        if (result.Home != null)
            await EnsureLinkedExpense(existing, result.Home.Id, null, null, null,
                result.Home.HomeValue.HasValue ? $"${result.Home.HomeValue.Value:N0}" : Localizer["Spending_AssetsExpenses_Home"].Value);

        // ── Investment Properties ─────────────────────────────────────────────
        foreach (var prop in result.InvestmentProperties)
            await EnsureLinkedExpense(existing, null, prop.Id, null, null,
                string.IsNullOrWhiteSpace(prop.Name) ? Localizer["Spending_AssetsExpenses_InvestmentProperty"].Value : prop.Name);

        // ── Investment Accounts ───────────────────────────────────────────────
        foreach (var account in result.InvestmentAccounts)
            await EnsureLinkedExpense(existing, null, null, account.Id, null,
                string.IsNullOrWhiteSpace(account.AccountName) ? Localizer["Spending_AssetsExpenses_InvestmentAccount"].Value : account.AccountName);

        // ── Physical Assets ───────────────────────────────────────────────────
        foreach (var asset in result.PhysicalAssets)
            await EnsureLinkedExpense(existing, null, null, null, asset.Id,
                string.IsNullOrWhiteSpace(asset.Name) ? Localizer["Spending_AssetsExpenses_PhysicalAsset"].Value : asset.Name);

        // ── Standalone (no asset link) ────────────────────────────────────────
        _standaloneItems = existing
            .Where(e => e.AssetsHomeId == null && e.AssetsInvestmentPropertyId == null
                     && e.AssetsInvestmentAccountId == null && e.AssetsPhysicalAssetId == null)
            .Select(e => new SpendingAssetsExpenseItemModel
            {
                Id          = e.Id,
                Name        = e.Name,
                Expense     = e.Expense,
                FrequencyId = e.FrequencyId,
            }).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task EnsureLinkedExpense(
        List<Domain.Entities.Dashboard.Spending.SpendingAssetsExpense> existing,
        long? homeId, long? propId, long? accountId, long? physicalId, string displayName)
    {
        var found = existing.FirstOrDefault(e =>
            e.AssetsHomeId == homeId &&
            e.AssetsInvestmentPropertyId == propId &&
            e.AssetsInvestmentAccountId == accountId &&
            e.AssetsPhysicalAssetId == physicalId);

        if (found == null)
        {
            var created = await Mediator.Send(new CreateAssetsExpenseCommand(
                ScenarioId, homeId, propId, accountId, physicalId));
            if (created.Success)
                _linkedItems.Add(new SpendingAssetsExpenseItemModel
                {
                    Id                          = created.ItemId,
                    Name                        = displayName,
                    FrequencyId                 = (int)FrequencyEnum.Monthly,
                    AssetsHomeId                = homeId,
                    AssetsInvestmentPropertyId  = propId,
                    AssetsInvestmentAccountId   = accountId,
                    AssetsPhysicalAssetId       = physicalId,
                });
        }
        else
        {
            _linkedItems.Add(new SpendingAssetsExpenseItemModel
            {
                Id                          = found.Id,
                Name                        = displayName,
                Expense                     = found.Expense,
                FrequencyId                 = found.FrequencyId,
                AssetsHomeId                = found.AssetsHomeId,
                AssetsInvestmentPropertyId  = found.AssetsInvestmentPropertyId,
                AssetsInvestmentAccountId   = found.AssetsInvestmentAccountId,
                AssetsPhysicalAssetId       = found.AssetsPhysicalAssetId,
            });
        }
    }

    private async Task SaveLinked(SpendingAssetsExpenseItemModel item)
    {
        await Mediator.Send(new UpdateAssetsExpenseCommand
        {
            Id                          = item.Id,
            Name                        = item.Name,
            Expense                     = item.Expense,
            FrequencyId                 = item.FrequencyId,
            AssetsHomeId                = item.AssetsHomeId,
            AssetsInvestmentPropertyId  = item.AssetsInvestmentPropertyId,
            AssetsInvestmentAccountId   = item.AssetsInvestmentAccountId,
            AssetsPhysicalAssetId       = item.AssetsPhysicalAssetId,
        });
    }

    private async Task AddStandalone()
    {
        var result = await Mediator.Send(new CreateAssetsExpenseCommand(ScenarioId));
        if (result.Success)
            _standaloneItems.Add(new SpendingAssetsExpenseItemModel
            {
                Id          = result.ItemId,
                FrequencyId = (int)FrequencyEnum.Monthly,
            });
    }

    private async Task RemoveStandalone(SpendingAssetsExpenseItemModel item)
    {
        await Mediator.Send(new DeleteAssetsExpenseCommand(item.Id));
        _standaloneItems.Remove(item);
    }

    private async Task SaveStandalone(SpendingAssetsExpenseItemModel item)
    {
        if (item.Id == 0) return;
        await Mediator.Send(new UpdateAssetsExpenseCommand
        {
            Id          = item.Id,
            Name        = item.Name,
            Expense     = item.Expense,
            FrequencyId = item.FrequencyId,
        });
    }

    private void NavigateNext() =>
        Navigation.NavigateTo($"/scenario/{ScenarioId}/spending/other-expenses");
}
