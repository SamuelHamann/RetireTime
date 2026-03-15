using MediatR;
using Microsoft.AspNetCore.Components;
using RetirementTime.Application.Features.BeginnerGuide.Debts.GetDebts;
using RetirementTime.Application.Features.BeginnerGuide.Debts.UpsertDebts;
using RetirementTime.Models.BeginnerGuide.Debts;

namespace RetirementTime.Components.Pages.BeginnerGuide.Debts;

public partial class Step1Debts
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    [Parameter]
    public EventCallback OnPrevious { get; set; }

    [Parameter]
    public EventCallback OnNext { get; set; }

    [Parameter]
    public long UserId { get; set; }

    private DebtFormModel _formModel = new();
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
            var existingDebts = await Mediator.Send(new GetDebtsQuery { UserId = UserId });
            if (existingDebts.Count > 0)
            {
                _formModel.HasDebts = true;
                _formModel.Debts = existingDebts.Select(d => new DebtItemModel
                {
                    Id = d.Id,
                    Type = d.Type,
                    Amount = d.Amount,
                    MonthlyPayment = d.MonthlyPayment,
                    InterestRate = d.InterestRate
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

    private void AddDebt()
    {
        _formModel.Debts.Add(new DebtItemModel());
        _errorMessage = null;
        _successMessage = null;
    }

    private void RemoveDebt(int index)
    {
        _formModel.Debts.RemoveAt(index);
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
            var command = new UpsertDebtsCommand
            {
                UserId = UserId,
                HasDebts = _formModel.HasDebts,
                Debts = _formModel.Debts.Select(d => new DebtInputDto
                {
                    Id = d.Id,
                    Type = d.Type,
                    Amount = d.Amount,
                    MonthlyPayment = d.MonthlyPayment,
                    InterestRate = d.InterestRate
                }).ToList()
            };

            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                _errorMessage = result.ErrorMessage ?? "Failed to save debts.";
                return;
            }

            await OnNext.InvokeAsync();
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

