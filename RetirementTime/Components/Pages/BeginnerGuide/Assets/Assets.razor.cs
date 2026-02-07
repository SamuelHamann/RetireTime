using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RetirementTime.Application.Features.BeginnerGuide.Assets.GetMainResidence;
using RetirementTime.Models.BeginnerGuide.Assets;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.BeginnerGuide.Assets;

public partial class Assets
{
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private AuthService AuthService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    private int _currentStep;
    private bool _ownsMainResidence;
    private PropertyData _propertyData = new();
    private bool _isLoading = true;
    private long _userId;

    protected override async Task OnInitializedAsync()
    {
        await LoadExistingData();
        _isLoading = false;
    }

    private async Task LoadExistingData()
    {
        try
        {
            var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
            if (authenticatedUser == null)
            {
                return;
            }

            _userId = authenticatedUser.UserId;

            // Fetch existing main residence data
            var query = new GetMainResidenceQuery { UserId = _userId };
            var result = await Mediator.Send(query);

            if (result.MainResidence != null)
            {
                // Populate the form with existing data
                _ownsMainResidence = result.MainResidence.HasMainResidence;

                if (result.MainResidence.HasMainResidence)
                {
                    _propertyData.PurchasePrice = result.MainResidence.PurchasePrice ?? 0;
                    _propertyData.MonthlyMortgagePayments = result.MainResidence.MonthlyMortgagePayments ?? 0;
                    _propertyData.MortgageLeft = result.MainResidence.MortgageLeft ?? 0;
                    _propertyData.YearlyInsurance = result.MainResidence.YearlyInsurance ?? 0;
                    _propertyData.MonthlyElectricityCosts = result.MainResidence.MonthlyElectricityCosts;
                    _propertyData.MortgageDuration = result.MainResidence.MortgageDuration ?? 0;
                    _propertyData.MortgageStartDate = result.MainResidence.MortgageStartDate;
                    _propertyData.EstimatedValue = result.MainResidence.EstimatedValue;
                }
            }
        }
        catch (Exception)
        {
            // Silently fail and start with empty form
            // Error is logged in the handler
        }
    }

    private void SetCurrentStep(int step)
    {
        _currentStep = step;
    }

    private string GetDotActiveClass(int step)
    {
        return step == _currentStep ? "active" : "";
    }

    private void GoToNextStep()
    {
        // Move to next step after successful save
        SetCurrentStep(_currentStep + 1);
    }
}

