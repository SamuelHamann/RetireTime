using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RetirementTime.Application.Features.BeginnerGuide.Assets.UpsertMainResidence;
using RetirementTime.Models.BeginnerGuide.Assets;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.BeginnerGuide.Assets;

public partial class Step1MainResidence
{
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private AuthService AuthService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }
    
    [Parameter] public bool OwnsMainResidence { get; set; }
    [Parameter] public EventCallback<bool> OwnsMainResidenceChanged { get; set; }
    [Parameter] public PropertyData PropertyData { get; set; } = new();
    [Parameter] public bool IsSaving { get; set; }
    [Parameter] public string? ErrorMessage { get; set; }
    [Parameter] public EventCallback OnSaveClicked { get; set; }
    [Parameter] public EventCallback OnNextClicked { get; set; }
    
    private bool _isSaving;
    private string? _errorMessage;

    private async Task HandleSave()
    {
        _errorMessage = null;

        // Validate required fields if user owns main residence
        if (OwnsMainResidence)
        {
            if (PropertyData.PurchasePrice <= 0 || PropertyData.MonthlyMortgagePayments <= 0 ||
                PropertyData.MortgageLeft <= 0 || PropertyData.YearlyInsurance <= 0 ||
                PropertyData.MortgageDuration <= 0 || !PropertyData.MortgageStartDate.HasValue)
            {
                _errorMessage = "Please fill in all required fields.";
                return;
            }
        }

        _isSaving = true;

        try
        {
            var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
            if (authenticatedUser == null)
            {
                _errorMessage = "Session expired. Please log in again.";
                _isSaving = false;
                return;
            }

            // Create the command
            var command = new UpsertMainResidenceCommand
            {
                UserId = authenticatedUser.UserId,
                HasMainResidence = OwnsMainResidence,
                PurchasePrice = OwnsMainResidence ? PropertyData.PurchasePrice : null,
                MonthlyMortgagePayments = OwnsMainResidence ? PropertyData.MonthlyMortgagePayments : null,
                MortgageLeft = OwnsMainResidence ? PropertyData.MortgageLeft : null,
                YearlyInsurance = OwnsMainResidence ? PropertyData.YearlyInsurance : null,
                MonthlyElectricityCosts = OwnsMainResidence ? PropertyData.MonthlyElectricityCosts : null,
                MortgageDuration = OwnsMainResidence ? PropertyData.MortgageDuration : null,
                MortgageStartDate = OwnsMainResidence ? PropertyData.MortgageStartDate : null,
                EstimatedValue = OwnsMainResidence ? PropertyData.EstimatedValue : null
            };

            var result = await Mediator.Send(command);

            if (result.Success)
            {
                // Notify parent to move to next step
                await OnSaveClicked.InvokeAsync();
            }
            else
            {
                _errorMessage = result.ErrorMessage;
            }
        }
        catch (Exception)
        {
            _errorMessage = "An unexpected error occurred. Please try again.";
        }
        finally
        {
            _isSaving = false;
        }
    }

    private async Task HandleNext()
    {
        // Save with HasMainResidence = false
        await HandleSave();
    }
}

