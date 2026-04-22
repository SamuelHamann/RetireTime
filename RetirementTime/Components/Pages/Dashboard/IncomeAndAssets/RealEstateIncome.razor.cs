using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Asset.GetAssetsInvestmentProperties;
using RetirementTime.Application.Features.Dashboard.PersistingIncome.GetRealEstateIncomes;
using RetirementTime.Application.Features.Dashboard.PersistingIncome.CreateRealEstateIncome;
using RetirementTime.Application.Features.Dashboard.PersistingIncome.UpdateRealEstateIncome;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Models.Income;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class RealEstateIncome : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private bool _isLoading = true;
    private List<AssetsInvestmentProperty> _properties = [];
    // One model per property (key = InvestmentPropertyId); Id == 0 means not yet saved to DB
    private Dictionary<long, RealEstateIncomeItemModel> _itemsByProperty = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var properties    = await Mediator.Send(new GetAssetsInvestmentPropertiesQuery(ScenarioId));
        var existingItems = await Mediator.Send(new GetRealEstateIncomesQuery(ScenarioId));

        _properties = properties;

        // Build lookup: one slot per property — use existing record or a fresh unsaved model
        foreach (var property in _properties)
        {
            var existing = existingItems.FirstOrDefault(e => e.InvestmentPropertyId == property.Id);
            _itemsByProperty[property.Id] = existing is not null
                ? new RealEstateIncomeItemModel
                {
                    Id                   = existing.Id,
                    InvestmentPropertyId = existing.InvestmentPropertyId,
                    PropertyName         = existing.PropertyName,
                    Amount               = existing.Amount,
                    FrequencyId          = existing.FrequencyId
                }
                : new RealEstateIncomeItemModel
                {
                    Id                   = 0,
                    InvestmentPropertyId = property.Id,
                    PropertyName         = property.Name
                };
        }

        _isLoading = false;
        StateHasChanged();
    }

    /// <summary>Lazy-creates the DB record on first save, then updates on subsequent saves.</summary>
    private async Task SaveIncome(RealEstateIncomeItemModel item)
    {
        if (item.InvestmentPropertyId is null) return;

        if (item.Id == 0)
        {
            var created = await Mediator.Send(new CreateRealEstateIncomeCommand(
                ScenarioId,
                item.InvestmentPropertyId,
                item.PropertyName));
            if (!created.Success) return;
            item.Id = created.IncomeId;
        }

        await Mediator.Send(new UpdateRealEstateIncomeCommand
        {
            Id                   = item.Id,
            InvestmentPropertyId = item.InvestmentPropertyId,
            PropertyName         = item.PropertyName,
            Amount               = item.Amount,
            FrequencyId          = item.FrequencyId
        });
    }
}
