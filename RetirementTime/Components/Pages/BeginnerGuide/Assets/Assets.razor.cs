using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RetirementTime.Application.Features.BeginnerGuide.Assets.GetMainResidence;
using RetirementTime.Application.Features.BeginnerGuide.Assets.GetInvestmentAccounts;
using RetirementTime.Application.Features.Locations.GetAccountTypes;
using RetirementTime.Domain.Entities.BeginnerGuide.Assets;
using RetirementTime.Models.BeginnerGuide.Assets;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.BeginnerGuide.Assets;

public partial class Assets
{
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    private int _currentStep;
    private bool _ownsMainResidence;
    private PropertyData _propertyData = new();
    private bool _isLoading = true;
    private long _userId;
    
    // Step 2: Investment Accounts
    private bool _hasInvestmentAccounts;
    private Step2InvestmentModel _investmentModel = new();
    private List<AccountType> _accountTypes = new();
    
    // Note: Step 3 (OtherAssets) and Step 4 (InvestmentProperties) load their own data
    // asynchronously in their respective OnInitializedAsync methods

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
            
            // Load account types
            var accountTypesQuery = new GetAccountTypesQuery { CountryId = 1 }; // TODO: Get from user's country
            var accountTypesResult = await Mediator.Send(accountTypesQuery);
            _accountTypes = accountTypesResult.AccountTypes.Select(a => new AccountType
            {
                Id = a.Id,
                Name = a.Name,
                CountryId = a.CountryId,
                Country = null!,
                SubdivisionId = null,
                Subdivision = null
            }).ToList();
            
            // Load existing investment accounts
            var investmentAccountsQuery = new GetInvestmentAccountsQuery { UserId = _userId };
            var investmentAccountsResult = await Mediator.Send(investmentAccountsQuery);

            if (investmentAccountsResult.Accounts.Any())
            {
                _hasInvestmentAccounts = true;
                _investmentModel.Accounts = investmentAccountsResult.Accounts.Select(a => new InvestmentAccountData
                {
                    Id = a.Id,
                    AccountName = a.AccountName,
                    AccountTypeId = a.AccountTypeId,
                    IsBulkAmount = a.IsBulkAmount,
                    BulkAmount = a.BulkAmount,
                    Stocks = a.Stocks.Select(s => new StockData
                    {
                        Id = s.Id,
                        TickerSymbol = s.TickerSymbol,
                        Amount = s.Amount
                    }).ToList()
                }).ToList();
            }
            else
            {
                // No existing accounts, ensure toggle is set to No
                _hasInvestmentAccounts = false;
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
        // If we're at the last step (Step 4, index 3), navigate to the next section
        if (_currentStep == 3)
        {
            Navigation.NavigateTo("/beginner-guide/debts");
            return;
        }
        
        // Otherwise, move to next step
        SetCurrentStep(_currentStep + 1);
    }

    private void GoToPreviousStep()
    {
        // Move to previous step
        if (_currentStep > 0)
        {
            SetCurrentStep(_currentStep - 1);
        }
    }
}

