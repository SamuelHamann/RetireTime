using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Asset.GetAssetsHome;
using RetirementTime.Application.Features.Dashboard.Asset.SaveAssetsHome;
using RetirementTime.Models.Asset;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class AssetsHome : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private AssetNavigationService AssetNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private bool _isLoading = true;
    private AssetsHomeModel _model = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var existing = await Mediator.Send(new GetAssetsHomeQuery(ScenarioId));
        if (existing != null)
        {
            _model = new AssetsHomeModel
            {
                PurchaseDate = existing.PurchaseDate,
                HomeValue = existing.HomeValue,
                PurchasePrice = existing.PurchasePrice
            };
        }

        _isLoading = false;
        StateHasChanged();
    }

    private async Task SaveAsync()
    {
        await Mediator.Send(new SaveAssetsHomeCommand
        {
            ScenarioId = ScenarioId,
            PurchaseDate = _model.PurchaseDate ?? DateTime.UtcNow,
            HomeValue = _model.HomeValue,
            PurchasePrice = _model.PurchasePrice
        });
    }

    private void NavigateNext() => AssetNavService.NavigateNext();
}
