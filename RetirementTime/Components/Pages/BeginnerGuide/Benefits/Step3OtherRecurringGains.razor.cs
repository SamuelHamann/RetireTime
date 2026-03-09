using Microsoft.AspNetCore.Components;
using RetirementTime.Models.BeginnerGuide.Benefits;

namespace RetirementTime.Components.Pages.BeginnerGuide.Benefits;

public partial class Step3OtherRecurringGains : ComponentBase
{
    [Parameter] public long UserId { get; set; }
    [Parameter] public EventCallback OnPrevious { get; set; }
    [Parameter] public EventCallback OnNext { get; set; }

    private bool _isLoading = false;
    private bool _isSaving = false;
    private bool _hasOtherRecurringGains = false;
    private List<OtherRecurringGainFormModel> _otherRecurringGains = new();
    private object _formData = new();

    // Temporary frequencies list - will be replaced with database data later
    private List<FrequencyDto> _frequencies = new()
    {
        new FrequencyDto { Id = 1, Name = "Monthly" },
        new FrequencyDto { Id = 2, Name = "Quarterly" },
        new FrequencyDto { Id = 3, Name = "Annually" },
        new FrequencyDto { Id = 4, Name = "One-time" }
    };

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        await Task.Delay(100); // Simulate loading
        // TODO: Load existing recurring gains from backend
        // TODO: Load frequencies from backend
        _isLoading = false;
    }

    private void AddRecurringGain()
    {
        _otherRecurringGains.Add(new OtherRecurringGainFormModel());
    }

    private void RemoveRecurringGain(int index)
    {
        if (index >= 0 && index < _otherRecurringGains.Count)
        {
            _otherRecurringGains.RemoveAt(index);
        }
    }

    private async Task HandleSaveAndComplete()
    {
        _isSaving = true;
        await Task.Delay(500); // Simulate saving
        // TODO: Save recurring gains to backend
        _isSaving = false;
        await OnNext.InvokeAsync();
    }

    private async Task HandleComplete()
    {
        await OnNext.InvokeAsync();
    }

    private async Task HandlePrevious()
    {
        await OnPrevious.InvokeAsync();
    }

    // Temporary DTO - will be replaced with actual backend DTO
    private class FrequencyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
