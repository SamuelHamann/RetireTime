using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace RetirementTime.Components.Shared;

public partial class YesNoToggle
{
    [Parameter]
    public bool Value { get; set; }
    
    [Parameter]
    public EventCallback<bool> ValueChanged { get; set; }
    
    [Parameter]
    public string YesLabel { get; set; } = "Yes";
    
    [Parameter]
    public string NoLabel { get; set; } = "No";

    private async Task SelectYes()
    {
        Value = true;
        await ValueChanged.InvokeAsync(Value);
    }

    private async Task SelectNo()
    {
        Value = false;
        await ValueChanged.InvokeAsync(Value);
    }

    private string GetYesClass()
    {
        return Value ? "selected selected-yes" : "";
    }

    private string GetNoClass()
    {
        return !Value ? "selected selected-no" : "";
    }
}

