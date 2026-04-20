using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Asset.CreatePhysicalAsset;
using RetirementTime.Application.Features.Dashboard.Asset.DeletePhysicalAsset;
using RetirementTime.Application.Features.Dashboard.Asset.GetPhysicalAssets;
using RetirementTime.Application.Features.Dashboard.Asset.UpdatePhysicalAsset;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Models.Asset;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class PhysicalAssets : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private bool _isLoading = true;
    private List<AssetsPhysicalAssetItemModel> _assetItems = [];
    private List<PhysicalAssetType> _assetTypes = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var result = await Mediator.Send(new GetPhysicalAssetsQuery(ScenarioId));
        _assetTypes = result.AssetTypes;
        _assetItems = result.Assets.Select(a => new AssetsPhysicalAssetItemModel
        {
            Id = a.Id,
            AssetTypeId = a.AssetTypeId,
            Name = a.Name,
            EstimatedValue = a.EstimatedValue,
            AdjustedCostBasis = a.AdjustedCostBasis,
            IsConsideredPersonalProperty = a.IsConsideredPersonalProperty,
            IsConsideredAsARetirementAsset = a.IsConsideredAsARetirementAsset
        }).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task AddAsset()
    {
        var result = await Mediator.Send(new CreatePhysicalAssetCommand(ScenarioId));
        if (result.Success)
            _assetItems.Add(new AssetsPhysicalAssetItemModel
            {
                Id = result.AssetId,
                AssetTypeId = (long)AssetTypeEnum.Other
            });
    }

    private async Task RemoveAsset(AssetsPhysicalAssetItemModel asset)
    {
        await Mediator.Send(new DeletePhysicalAssetCommand(asset.Id));
        _assetItems.Remove(asset);
    }

    private async Task SaveAsset(AssetsPhysicalAssetItemModel asset)
    {
        if (asset.Id == 0) return;
        await Mediator.Send(new UpdatePhysicalAssetCommand
        {
            Id = asset.Id,
            AssetTypeId = asset.AssetTypeId,
            Name = asset.Name,
            EstimatedValue = asset.EstimatedValue,
            AdjustedCostBasis = asset.AdjustedCostBasis,
            IsConsideredPersonalProperty = asset.IsConsideredPersonalProperty,
            IsConsideredAsARetirementAsset = asset.IsConsideredAsARetirementAsset
        });
    }
}
