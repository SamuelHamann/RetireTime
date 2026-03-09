using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components;
using RetirementTime.Application.Features.BeginnerGuide.Assets.GetAssetTypes;
using RetirementTime.Application.Features.BeginnerGuide.Assets.GetOtherAssets;
using RetirementTime.Application.Features.BeginnerGuide.Assets.UpsertOtherAssets;
using RetirementTime.Models.BeginnerGuide.Assets;

namespace RetirementTime.Components.Pages.BeginnerGuide.Assets;

public partial class Step3OtherAssets
{
    [Inject] private IMediator Mediator { get; set; } = default!;
    
    [Parameter]
    public EventCallback OnPrevious { get; set; }
    
    [Parameter]
    public EventCallback OnNext { get; set; }
    
    [Parameter]
    public long UserId { get; set; }

    private OtherAssetFormModel _formModel = new();
    private List<AssetTypeDto> _assetTypes = new();
    private string? _errorMessage;
    private string? _successMessage;
    private bool _isSubmitting;
    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        _isLoading = true;
        
        try
        {
            // Load asset types and existing assets in parallel for better performance
            var assetTypesTask = Mediator.Send(new GetAssetTypesQuery());
            var existingAssetsTask = Mediator.Send(new GetOtherAssetsQuery { UserId = UserId });

            await Task.WhenAll(assetTypesTask, existingAssetsTask);

            _assetTypes = await assetTypesTask;
            var existingAssets = await existingAssetsTask;
            
            if (existingAssets.Count > 0)
            {
                _formModel.HasOtherAssets = true;
                _formModel.Assets = existingAssets.Select(a => new OtherAssetItemModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    AssetTypeId = a.AssetTypeId,
                    CurrentValue = a.CurrentValue,
                    PurchasePrice = a.PurchasePrice
                }).ToList();
            }
        }
        catch (Exception)
        {
            _errorMessage = "Failed to load data. Please refresh the page.";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void AddAsset()
    {
        _formModel.Assets.Add(new OtherAssetItemModel());
        _errorMessage = null;
        _successMessage = null;
    }

    private void RemoveAsset(int index)
    {
        _formModel.Assets.RemoveAt(index);
        _errorMessage = null;
        _successMessage = null;
    }

    private async Task HandleSubmit()
    {
        _isSubmitting = true;
        _errorMessage = null;
        _successMessage = null;

        try
        {
            var command = new UpsertOtherAssetsCommand
            {
                UserId = UserId,
                HasOtherAssets = _formModel.HasOtherAssets,
                Assets = _formModel.Assets.Select(a => new OtherAssetInputDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    AssetTypeId = a.AssetTypeId,
                    CurrentValue = a.CurrentValue,
                    PurchasePrice = a.PurchasePrice
                }).ToList()
            };

            var result = await Mediator.Send(command);

            if (result.Success)
            {
                _successMessage = "Assets saved successfully!";
                await Task.Delay(500); // Brief delay to show success message
                await OnNext.InvokeAsync();
            }
            else
            {
                _errorMessage = result.ErrorMessage ?? "Failed to save assets.";
            }
        }
        catch (Exception)
        {
            _errorMessage = "An unexpected error occurred. Please try again.";
        }
        finally
        {
            _isSubmitting = false;
        }
    }

    private async Task GoBack()
    {
        await OnPrevious.InvokeAsync();
    }
}

