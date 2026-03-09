using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components;
using RetirementTime.Application.Features.BeginnerGuide.Assets.GetInvestmentProperties;
using RetirementTime.Application.Features.BeginnerGuide.Assets.UpsertInvestmentProperties;
using RetirementTime.Models.BeginnerGuide.Assets;

namespace RetirementTime.Components.Pages.BeginnerGuide.Assets;

public partial class Step4InvestmentProperties
{
    [Inject] private IMediator Mediator { get; set; } = default!;
    
    [Parameter]
    public EventCallback OnPrevious { get; set; }
    
    [Parameter]
    public EventCallback OnNext { get; set; }
    
    [Parameter]
    public long UserId { get; set; }

    private InvestmentPropertyFormModel _formModel = new();
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
            var existingProperties = await Mediator.Send(new GetInvestmentPropertiesQuery { UserId = UserId });
            
            if (existingProperties.Count > 0)
            {
                _formModel.HasInvestmentProperties = true;
                _formModel.Properties = existingProperties.Select(p => new InvestmentPropertyItemModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    PurchasePrice = p.PurchasePrice,
                    MonthlyMortgagePayments = p.MonthlyMortgagePayments,
                    MortgageLeft = p.MortgageLeft,
                    YearlyInsurance = p.YearlyInsurance,
                    MonthlyElectricityCosts = p.MonthlyElectricityCosts,
                    MortgageDuration = p.MortgageDuration,
                    MortgageStartDate = p.MortgageStartDate,
                    EstimatedValue = p.EstimatedValue,
                    MonthlyCost = p.MonthlyCost,
                    MonthlyRevenue = p.MonthlyRevenue
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

    private void AddProperty()
    {
        _formModel.Properties.Add(new InvestmentPropertyItemModel());
        _errorMessage = null;
        _successMessage = null;
    }

    private void RemoveProperty(int index)
    {
        _formModel.Properties.RemoveAt(index);
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
            var command = new UpsertInvestmentPropertiesCommand
            {
                UserId = UserId,
                HasInvestmentProperties = _formModel.HasInvestmentProperties,
                Properties = _formModel.Properties.Select(p => new InvestmentPropertyInputDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    PurchasePrice = p.PurchasePrice,
                    MonthlyMortgagePayments = p.MonthlyMortgagePayments,
                    MortgageLeft = p.MortgageLeft,
                    YearlyInsurance = p.YearlyInsurance,
                    MonthlyElectricityCosts = p.MonthlyElectricityCosts,
                    MortgageDuration = p.MortgageDuration,
                    MortgageStartDate = p.MortgageStartDate,
                    EstimatedValue = p.EstimatedValue,
                    MonthlyCost = p.MonthlyCost,
                    MonthlyRevenue = p.MonthlyRevenue
                }).ToList()
            };

            var result = await Mediator.Send(command);

            if (result.Success)
            {
                _successMessage = "Investment properties saved successfully!";
                await Task.Delay(500); // Brief delay to show success message
                await OnNext.InvokeAsync();
            }
            else
            {
                _errorMessage = result.ErrorMessage ?? "Failed to save investment properties.";
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

