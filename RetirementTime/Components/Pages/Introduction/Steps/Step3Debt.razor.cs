using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RetirementTime.Models.Introduction;
using RetirementTime.Resources.Introduction;

namespace RetirementTime.Components.Pages.Introduction.Steps;

public partial class Step3Debt
{
    [Inject] private IStringLocalizer<IntroResources> Localizer { get; set; } = default!;

    public DebtModel Model { get; set; } = new();
}

