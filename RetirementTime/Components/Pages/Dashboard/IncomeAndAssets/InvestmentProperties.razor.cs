using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Asset.CreateAssetsInvestmentProperty;
using RetirementTime.Application.Features.Dashboard.Asset.DeleteAssetsInvestmentProperty;
using RetirementTime.Application.Features.Dashboard.Asset.GetAssetsInvestmentProperties;
using RetirementTime.Application.Features.Dashboard.Asset.UpdateAssetsInvestmentProperty;
using RetirementTime.Models.Asset;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class InvestmentProperties : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private AssetNavigationService AssetNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private bool _isLoading = true;
    private List<AssetsInvestmentPropertyItemModel> _propertyItems = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var items = await Mediator.Send(new GetAssetsInvestmentPropertiesQuery(ScenarioId));
        _propertyItems = items.Select(e => new AssetsInvestmentPropertyItemModel
        {
            Id = e.Id,
            Name = e.Name,
            PurchaseDate = e.PurchaseDate,
            PropertyValue = e.PropertyValue,
            PurchasePrice = e.PurchasePrice
        }).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task AddProperty()
    {
        var result = await Mediator.Send(new CreateAssetsInvestmentPropertyCommand(ScenarioId));
        if (result.Success) _propertyItems.Add(new AssetsInvestmentPropertyItemModel { Id = result.PropertyId });
    }

    private async Task RemoveProperty(AssetsInvestmentPropertyItemModel item)
    {
        await Mediator.Send(new DeleteAssetsInvestmentPropertyCommand(item.Id));
        _propertyItems.Remove(item);
    }

    private async Task SaveProperty(AssetsInvestmentPropertyItemModel item)
    {
        if (item.Id == 0) return;
        await Mediator.Send(new UpdateAssetsInvestmentPropertyCommand
        {
            Id = item.Id,
            Name = item.Name,
            PurchaseDate = item.PurchaseDate ?? DateTime.UtcNow,
            PropertyValue = item.PropertyValue,
            PurchasePrice = item.PurchasePrice
        });
    }

    private void NavigateNext() => AssetNavService.NavigateNext();
}
