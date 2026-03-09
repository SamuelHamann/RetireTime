using Microsoft.AspNetCore.Components;
using RetirementTime.Models.BeginnerGuide.Benefits;

namespace RetirementTime.Components.Pages.BeginnerGuide.Benefits;

public partial class Step1Pension : ComponentBase
{
    [Parameter] public long UserId { get; set; }
    [Parameter] public EventCallback OnNext { get; set; }
    [Parameter] public EventCallback OnPrevious { get; set; }

    private bool _isLoading = false;
    private bool _isSaving = false;
    private bool _hasPensions = false;
    private List<PensionFormModel> _pensions = new();
    private object _formData = new();

    // Temporary pension types list - will be replaced with database data later
    private List<PensionTypeDto> _pensionTypes = new()
    {
        new PensionTypeDto { Id = 1, Name = "Defined Benefit Pension Plan (DBPP)" },
        new PensionTypeDto { Id = 2, Name = "Defined Contribution Pension Plan (DCPP)" },
        new PensionTypeDto { Id = 3, Name = "Pooled Registered Pension Plan (PRPP)" },
        new PensionTypeDto { Id = 4, Name = "Group Registered Retirement Savings Plan (GRSP)" },
        new PensionTypeDto { Id = 5, Name = "Target Benefit Plan" },
        new PensionTypeDto { Id = 6, Name = "Deferred Profit Sharing Plan (DPSP)" }
    };

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        await Task.Delay(100); // Simulate loading
        // TODO: Load existing pensions from backend
        _isLoading = false;
    }

    private void AddPension()
    {
        _pensions.Add(new PensionFormModel());
    }

    private void RemovePension(int index)
    {
        if (index >= 0 && index < _pensions.Count)
        {
            _pensions.RemoveAt(index);
        }
    }

    private async Task HandleSaveAndContinue()
    {
        _isSaving = true;
        await Task.Delay(500); // Simulate saving
        // TODO: Save pensions to backend
        _isSaving = false;
        await OnNext.InvokeAsync();
    }

    private async Task HandleNext()
    {
        await OnNext.InvokeAsync();
    }

    private async Task HandlePrevious()
    {
        await OnPrevious.InvokeAsync();
    }

    // Temporary DTO - will be replaced with actual backend DTO
    private class PensionTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
