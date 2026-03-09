using Microsoft.AspNetCore.Components;
using RetirementTime.Models.BeginnerGuide.Benefits;

namespace RetirementTime.Components.Pages.BeginnerGuide.Benefits;

public partial class Step2GovernmentPension : ComponentBase
{
    [Parameter] public long UserId { get; set; }
    [Parameter] public EventCallback OnNext { get; set; }
    [Parameter] public EventCallback OnPrevious { get; set; }

    private bool _isLoading = false;
    private bool _isSaving = false;
    private GovernmentPensionFormModel _governmentPension = new();

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        await Task.Delay(100); // Simulate loading
        // TODO: Load existing government pension data from backend
        _isLoading = false;
    }

    private async Task HandleSaveAndContinue()
    {
        _isSaving = true;
        await Task.Delay(500); // Simulate saving
        // TODO: Save government pension data to backend
        _isSaving = false;
        await OnNext.InvokeAsync();
    }

    private async Task HandlePrevious()
    {
        await OnPrevious.InvokeAsync();
    }
}
