using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RetirementTime.Application.Features.BeginnerGuide.Assets.UpsertInvestmentAccounts;
using RetirementTime.Domain.Entities.BeginnerGuide.Assets;
using RetirementTime.Models.BeginnerGuide.Assets;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.BeginnerGuide.Assets;

public partial class Step2InvestmentAccounts
{
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private AuthService AuthService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }
    
    [Parameter] public bool HasInvestmentAccounts { get; set; }
    [Parameter] public EventCallback<bool> HasInvestmentAccountsChanged { get; set; }
    [Parameter] public Step2InvestmentModel Model { get; set; } = new();
    [Parameter] public List<AccountType> AccountTypes { get; set; } = new();
    [Parameter] public EventCallback OnSaveClicked { get; set; }
    
    private bool _isSaving;
    private string? _errorMessage;

    private void AddAccount()
    {
        Model.Accounts.Add(new InvestmentAccountData
        {
            IsBulkAmount = true,
            Stocks = new List<StockData> { new StockData() }
        });
    }

    private void RemoveAccount(int index)
    {
        if (Model.Accounts.Count > 1)
        {
            Model.Accounts.RemoveAt(index);
        }
    }

    private void SetBulkAmount(int accountIndex, bool isBulk)
    {
        Model.Accounts[accountIndex].IsBulkAmount = isBulk;
        if (!isBulk && Model.Accounts[accountIndex].Stocks.Count == 0)
        {
            Model.Accounts[accountIndex].Stocks.Add(new StockData());
        }
    }

    private void AddStock(int accountIndex)
    {
        Model.Accounts[accountIndex].Stocks.Add(new StockData());
    }

    private void RemoveStock(int accountIndex, int stockIndex)
    {
        if (Model.Accounts[accountIndex].Stocks.Count > 1)
        {
            Model.Accounts[accountIndex].Stocks.RemoveAt(stockIndex);
        }
    }

    private async Task HandleSave()
    {
        _errorMessage = null;

        // Validate all accounts using data annotations
        if (HasInvestmentAccounts)
        {
            foreach (var account in Model.Accounts)
            {
                var validationContext = new ValidationContext(account);
                var validationResults = new List<ValidationResult>();
                
                if (!Validator.TryValidateObject(account, validationContext, validationResults, true))
                {
                    _errorMessage = validationResults[0].ErrorMessage ?? "Please fix validation errors.";
                    return;
                }

                // Validate individual stocks if not using bulk amount
                if (!account.IsBulkAmount)
                {
                    if (account.Stocks.Count == 0)
                    {
                        _errorMessage = "Please add at least one holding for accounts with individual holdings.";
                        return;
                    }

                    foreach (var stock in account.Stocks)
                    {
                        var stockValidationContext = new ValidationContext(stock);
                        var stockValidationResults = new List<ValidationResult>();
                        
                        if (!Validator.TryValidateObject(stock, stockValidationContext, stockValidationResults, true))
                        {
                            _errorMessage = stockValidationResults[0].ErrorMessage ?? "Please fix validation errors in holdings.";
                            return;
                        }
                    }
                }
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

            // Convert UI model to command DTOs
            var accountDtos = Model.Accounts.Select(a => new Application.Features.BeginnerGuide.Assets.UpsertInvestmentAccounts.InvestmentAccountDto
            {
                Id = a.Id,
                AccountName = a.AccountName,
                AccountTypeId = a.AccountTypeId,
                IsBulkAmount = a.IsBulkAmount,
                BulkAmount = a.BulkAmount,
                Stocks = a.Stocks.Select(s => new Application.Features.BeginnerGuide.Assets.UpsertInvestmentAccounts.StockDto
                {
                    Id = s.Id,
                    TickerSymbol = s.TickerSymbol,
                    Amount = s.Amount
                }).ToList()
            }).ToList();

            var command = new UpsertInvestmentAccountsCommand
            {
                UserId = authenticatedUser.UserId,
                Accounts = accountDtos
            };

            var result = await Mediator.Send(command);

            if (!result.Success)
            {
                _errorMessage = result.ErrorMessage ?? "Failed to save investment accounts.";
                _isSaving = false;
                return;
            }

            await OnSaveClicked.InvokeAsync();
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
        // Save with no investment accounts
        await HandleSave();
    }
}

