using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RetirementTime.Resources.Common;

namespace RetirementTime.Components.Shared;

public partial class TopBar
{
    [Inject] private IStringLocalizer<CommonResources> CommonLocalizer { get; set; } = default!;

    [Parameter] public string ActiveSection { get; set; } = "introduction";
    [Parameter] public string Initials { get; set; } = "?";
    [Parameter] public bool HasCompletedIntro { get; set; }
}
