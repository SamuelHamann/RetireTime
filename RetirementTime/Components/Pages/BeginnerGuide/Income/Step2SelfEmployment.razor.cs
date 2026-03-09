using MediatR;
using Microsoft.AspNetCore.Components;
using RetirementTime.Application.Features.BeginnerGuide.Income.GetSelfEmployments;
using RetirementTime.Application.Features.BeginnerGuide.Income.UpsertSelfEmployments;
using RetirementTime.Application.Features.Common.GetFrequencies;
using RetirementTime.Models.BeginnerGuide.Income;

namespace RetirementTime.Components.Pages.BeginnerGuide.Income;

public partial class Step2SelfEmployment
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    [Parameter] public EventCallback OnPrevious { get; set; }
    [Parameter] public EventCallback OnNext { get; set; }
    [Parameter] public long UserId { get; set; }

    private SelfEmploymentFormModel _formModel = new();
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

            // Load existing self-employment data
            var query = new GetSelfEmploymentsQuery { UserId = UserId };
            var result = await Mediator.Send(query);

            if (result.Success && result.SelfEmployments.Any())
            {
                _formModel.IsSelfEmployed = true;
                _formModel.SelfEmployments = result.SelfEmployments.Select(e => new SelfEmploymentItemModel
                {
                    BusinessName = e.BusinessName,
                    AnnualSalary = e.AnnualSalary,
                    AverageAnnualRevenueIncrease = e.AverageAnnualRevenueIncrease,
                    MonthlyDividends = e.MonthlyDividends,
                    AdditionalCompensations = e.AdditionalCompensations.Select(c => new SelfEmploymentAdditionalCompensationItemModel
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

    private void AddSelfEmployment()
    {
        _formModel.SelfEmployments.Add(new SelfEmploymentItemModel());
        _errorMessage = null;
    }

    private void RemoveSelfEmployment(int index)
    {
        _formModel.SelfEmployments.RemoveAt(index);
        _errorMessage = null;
    }

    private void AddCompensation(int selfEmploymentIndex)
    {
        _formModel.SelfEmployments[selfEmploymentIndex].AdditionalCompensations.Add(new SelfEmploymentAdditionalCompensationItemModel());
        _errorMessage = null;
    }

    private void RemoveCompensation(int selfEmploymentIndex, int compensationIndex)
    {
        _formModel.SelfEmployments[selfEmploymentIndex].AdditionalCompensations.RemoveAt(compensationIndex);
        _errorMessage = null;
    }

    private async Task HandleSubmit()
    {
        _isSubmitting = true;
        _errorMessage = null;

        try
        {
            // If user is not self-employed, clear all self-employments
            var selfEmploymentDtos = new List<Application.Features.BeginnerGuide.Income.UpsertSelfEmployments.SelfEmploymentDto>();

            if (_formModel.IsSelfEmployed && _formModel.SelfEmployments.Any())
            {
                selfEmploymentDtos = _formModel.SelfEmployments.Select(e => new Application.Features.BeginnerGuide.Income.UpsertSelfEmployments.SelfEmploymentDto
                {
                    BusinessName = e.BusinessName,
                    AnnualSalary = e.AnnualSalary,
                    AverageAnnualRevenueIncrease = e.AverageAnnualRevenueIncrease,
                    MonthlyDividends = e.MonthlyDividends,
                    AdditionalCompensations = e.AdditionalCompensations.Select(c => new Application.Features.BeginnerGuide.Income.UpsertSelfEmployments.AdditionalCompensationDto
                    {
                        Name = c.Name,
                        Amount = c.Amount,
                        FrequencyId = c.FrequencyId
                    }).ToList()
                }).ToList();
            }

            var command = new UpsertSelfEmploymentsCommand
            {
                UserId = UserId,
                SelfEmployments = selfEmploymentDtos
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
