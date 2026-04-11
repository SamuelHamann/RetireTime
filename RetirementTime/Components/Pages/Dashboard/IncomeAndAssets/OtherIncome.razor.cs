using Microsoft.AspNetCore.Components;
using RetirementTime.Resources.Dashboard;
using Microsoft.Extensions.Localization;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class OtherIncome : ComponentBase
{
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;

    [Parameter] public long ScenarioId { get; set; }
}

