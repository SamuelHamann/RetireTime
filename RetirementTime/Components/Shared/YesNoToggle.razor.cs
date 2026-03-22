using Microsoft.AspNetCore.Components;

namespace RetirementTime.Components.Shared;

public partial class YesNoToggle
{
    [Parameter] public bool Value { get; set; }
    [Parameter] public EventCallback<bool> ValueChanged { get; set; }

    // Kept for backward compatibility
    [Parameter] public string YesLabel { get; set; } = "Yes";
    [Parameter] public string NoLabel { get; set; } = "No";

    private async Task Toggle()
    {
        Value = !Value;
        await ValueChanged.InvokeAsync(Value);
    }
}
