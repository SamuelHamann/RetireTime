using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using RetirementTime.Application.Features.Dashboard.Cashflow.GetCashflow;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace RetirementTime.Components.Pages.Dashboard.Overview;

public partial class Cashflow : ComponentBase, IAsyncDisposable
{
    [Inject] private IJSRuntime Js { get; set; } = null!;
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private AuthService AuthService { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = null!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private IJSObjectReference? _module;
    private GetCashflowResult? _cashflowResult;
    private bool _isMonthly = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        _cashflowResult = await Mediator.Send(new GetCashflowQuery
        {
            ScenarioId                   = ScenarioId,
            Label_Employment             = Localizer["Cashflow_Label_Employment"],
            Label_SelfEmployment         = Localizer["Cashflow_Label_SelfEmployment"],
            Label_DefinedBenefits        = Localizer["Cashflow_Label_DefinedBenefits"],
            Label_OtherIncome            = Localizer["Cashflow_Label_OtherIncome"],
            Label_RealEstateIncome       = Localizer["Cashflow_Label_RealEstateIncome"],
            Label_OtherPersistingIncome  = Localizer["Cashflow_Label_OtherPersistingIncome"],
            Label_TotalIncome            = Localizer["Cashflow_Label_TotalIncome"],
            Label_LivingExpenses         = Localizer["Cashflow_Label_LivingExpenses"],
            Label_DiscretionaryExpenses  = Localizer["Cashflow_Label_DiscretionaryExpenses"],
            Label_DebtRepayments         = Localizer["Cashflow_Label_DebtRepayments"],
            Label_AssetsExpenses         = Localizer["Cashflow_Label_AssetsExpenses"],
            Label_OtherExpenses          = Localizer["Cashflow_Label_OtherExpenses"],
            Label_TotalExpenses          = Localizer["Cashflow_Label_TotalExpenses"],
            Label_Savings                = Localizer["Cashflow_Label_Savings"],
        });

        _module = await Js.InvokeAsync<IJSObjectReference>("import", "./Components/Pages/Dashboard/Overview/Cashflow.razor.js");
        await RenderChart();
    }

    private async Task SetPeriod(bool monthly)
    {
        _isMonthly = monthly;
        await RenderChart();
    }

    private async Task RenderChart()
    {
        if (_module is null || _cashflowResult is null) return;

        var divisor = _isMonthly ? 12m : 1m;
        var scaled = new CashflowSankeyDto
        {
            Nodes = _cashflowResult.Data.Nodes,
            Links = _cashflowResult.Data.Links
                .Select(l => new CashflowLinkDto
                {
                    Source = l.Source,
                    Target = l.Target,
                    Value  = Math.Round(l.Value / divisor, 2)
                })
                .ToList()
        };

        await _module.InvokeVoidAsync("render", "cashflow-chart", scaled);

        // Bar chart — yearly projections (always annual, not divided)
        var barData = _cashflowResult.YearlyCashFlows
            .Select(y => new { year = y.Year, totalIncome = Math.Round(y.TotalIncome / divisor, 2), totalExpenses = Math.Round(y.TotalExpenses / divisor, 2) })
            .ToArray();

        await _module.InvokeVoidAsync("renderBarChart",
            "cashflow-bar-chart",
            barData,
            Localizer["Cashflow_Label_TotalIncome"].ToString(),
            Localizer["Cashflow_Label_TotalExpenses"].ToString(),
            Localizer["Cashflow_Label_Savings"].ToString());
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
            await _module.DisposeAsync();
    }
}
