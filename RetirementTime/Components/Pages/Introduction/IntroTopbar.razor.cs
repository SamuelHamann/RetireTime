using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RetirementTime.Resources.Common;

namespace RetirementTime.Components.Pages.Introduction;

public partial class IntroTopbar
{
    [Inject] private IStringLocalizer<CommonResources> CommonLocalizer { get; set; } = default!;

    [Parameter] public string ActiveSection { get; set; } = "introduction";
    [Parameter] public string Initials { get; set; } = "?";
}

