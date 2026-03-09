using MediatR;
using Microsoft.AspNetCore.Components;
using RetirementTime.Application.Features.BeginnerGuide.Income.GetEmployments;
using RetirementTime.Application.Features.BeginnerGuide.Income.UpsertEmployments;
using RetirementTime.Application.Features.Common.GetFrequencies;
using RetirementTime.Models.BeginnerGuide.Income;

namespace RetirementTime.Components.Pages.BeginnerGuide.Income;

public partial class Step1Employment
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    [Parameter] public EventCallback OnPrevious { get; set; }
    [Parameter] public EventCallback OnNext { get; set; }
    [Parameter] public long UserId { get; set; }

    private EmploymentFormModel _formModel = new();
    private List<FrequencyDto> _frequencies = new();
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
            // Load frequencies
            _frequencies = await Mediator.Send(new GetFrequenciesQuery());

            // Load existing employment data
            var query = new GetEmploymentsQuery { UserId = UserId };
            var result = await Mediator.Send(query);

            if (result.Success && result.Employments.Any())
            {
                _formModel.IsEmployed = true;
                _formModel.Employments = result.Employments.Select(e => new EmploymentItemModel
                {
                    EmployerName = e.EmployerName,
                    AnnualSalary = e.AnnualSalary,
                    AverageAnnualWageIncrease = e.AverageAnnualWageIncrease,
                    AdditionalCompensations = e.AdditionalCompensations.Select(c => new AdditionalCompensationItemModel
                    {
                        Name = c.Name,
                        Amount = c.Amount,
                        FrequencyId = c.FrequencyId
                    }).ToList()
                }).ToList();
            }
            else if (!result.Success)
            {
                _errorMessage = result.ErrorMessage;
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
            // If user is not employed, clear all employments
            var employmentDtos = new List<Application.Features.BeginnerGuide.Income.UpsertEmployments.EmploymentDto>();

            if (_formModel.IsEmployed && _formModel.Employments.Any())
            {
                employmentDtos = _formModel.Employments.Select(e => new Application.Features.BeginnerGuide.Income.UpsertEmployments.EmploymentDto
                {
                    EmployerName = e.EmployerName,
                    AnnualSalary = e.AnnualSalary,
                    AverageAnnualWageIncrease = e.AverageAnnualWageIncrease,
                    AdditionalCompensations = e.AdditionalCompensations.Select(c => new Application.Features.BeginnerGuide.Income.UpsertEmployments.AdditionalCompensationDto
                    {
                        Name = c.Name,
                        Amount = c.Amount,
                        FrequencyId = c.FrequencyId
                    }).ToList()
                }).ToList();
            }

            var command = new UpsertEmploymentsCommand
            {
                UserId = UserId,
                Employments = employmentDtos
            };

            var result = await Mediator.Send(command);

            if (result.Success)
            {
                await OnNext.InvokeAsync();
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
            _isSubmitting = false;
        }
    }

    private async Task GoBack()
    {
        await OnPrevious.InvokeAsync();
    }
}

