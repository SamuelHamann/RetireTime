using Microsoft.AspNetCore.Components;
using RetirementTime.Models.BeginnerGuide.Income;

namespace RetirementTime.Components.Pages.BeginnerGuide.Income;

public partial class Step1Employment
{
    [Parameter] public EventCallback OnPrevious { get; set; }
    [Parameter] public EventCallback OnNext { get; set; }
    [Parameter] public long UserId { get; set; }

    private EmploymentFormModel _formModel = new();
    private string? _errorMessage;
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
            // TODO: load existing employment data via Mediator
            await Task.CompletedTask;
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

    private void AddEmployment()
    {
        _formModel.Employments.Add(new EmploymentItemModel());
        _errorMessage = null;
    }

    private void RemoveEmployment(int index)
    {
        _formModel.Employments.RemoveAt(index);
        _errorMessage = null;
    }

    private void AddCompensation(int employmentIndex)
    {
        _formModel.Employments[employmentIndex].AdditionalCompensations.Add(new AdditionalCompensationItemModel());
        _errorMessage = null;
    }

    private void RemoveCompensation(int employmentIndex, int compensationIndex)
    {
        _formModel.Employments[employmentIndex].AdditionalCompensations.RemoveAt(compensationIndex);
        _errorMessage = null;
    }

    private async Task HandleSubmit()
    {
        _isSubmitting = true;
        _errorMessage = null;

        try
        {
            // TODO: send UpsertEmploymentCommand via Mediator
            await Task.CompletedTask;

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

