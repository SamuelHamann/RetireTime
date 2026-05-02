using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Common.GetFrequencies;
using RetirementTime.Application.Features.Dashboard.Income.CreateEmploymentIncome;
using RetirementTime.Application.Features.Dashboard.Income.CreateOtherIncome;
using RetirementTime.Application.Features.Dashboard.Income.DeleteEmploymentIncome;
using RetirementTime.Application.Features.Dashboard.Income.DeleteOtherIncome;
using RetirementTime.Application.Features.Dashboard.Income.GetEmploymentIncomes;
using RetirementTime.Application.Features.Dashboard.Income.UpdateEmploymentIncome;
using RetirementTime.Application.Features.Dashboard.Income.UpdateOtherIncome;
using RetirementTime.Models.Income;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;


namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class Employment : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private IncomeNavigationService IncomeNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private long _userId;

    private bool _isLoading = true;
    private List<EmploymentItemModel> _employmentItems = [];
    private List<FrequencyDto> _frequencies = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        _userId = authenticatedUser.UserId;

        _frequencies = await Mediator.Send(new GetFrequenciesQuery());
        var employments = await Mediator.Send(new GetEmploymentIncomesQuery(ScenarioId));

        _employmentItems = employments.Select(e => new EmploymentItemModel
        {
            Id = e.Id,
            EmployerName = e.EmployerName,
            GrossSalary = e.GrossSalary,
            GrossSalaryFrequencyId = e.GrossSalaryFrequencyId,
            NetSalary = e.NetSalary,
            NetSalaryFrequencyId = e.NetSalaryFrequencyId,
            GrossCommissions = e.GrossCommissions,
            GrossCommissionsFrequencyId = e.GrossCommissionsFrequencyId,
            NetCommissions = e.NetCommissions,
            NetCommissionsFrequencyId = e.NetCommissionsFrequencyId,
            GrossBonus = e.GrossBonus,
            GrossBonusFrequencyId = e.GrossBonusFrequencyId,
            NetBonus = e.NetBonus,
            NetBonusFrequencyId = e.NetBonusFrequencyId,
            OtherIncomes = e.OtherIncomes.Select(o => new OtherEmploymentIncomeItemModel
            {
                Id = o.Id,
                Name = o.Name,
                Gross = o.Gross,
                GrossFrequencyId = o.GrossFrequencyId,
                Net = o.Net,
                NetFrequencyId = o.NetFrequencyId
            }).ToList()
        }).ToList();

        _isLoading = false;
        StateHasChanged();
    }


    private async Task AddEmployment()
    {
        var result = await Mediator.Send(new CreateEmploymentIncomeCommand(ScenarioId, _userId));
        if (result.Success)
        {
            _employmentItems.Add(new EmploymentItemModel { Id = result.EmploymentIncomeId });
        }
    }

    private async Task RemoveEmployment(EmploymentItemModel item)
    {
        await Mediator.Send(new DeleteEmploymentIncomeCommand(item.Id));
        _employmentItems.Remove(item);
    }

    private async Task AddOtherIncome(EmploymentItemModel item)
    {
        var result = await Mediator.Send(new CreateOtherIncomeCommand(item.Id));
        if (result.Success)
        {
            item.OtherIncomes.Add(new OtherEmploymentIncomeItemModel { Id = result.OtherIncomeId });
        }
    }

    private async Task RemoveOtherIncome(EmploymentItemModel item, OtherEmploymentIncomeItemModel other)
    {
        await Mediator.Send(new DeleteOtherIncomeCommand(other.Id));
        item.OtherIncomes.Remove(other);
    }

    private async Task SaveEmployment(EmploymentItemModel item)
    {
        if (item.Id == 0) return;

        await Mediator.Send(new UpdateEmploymentIncomeCommand
        {
            Id = item.Id,
            EmployerName = item.EmployerName,
            GrossSalary = item.GrossSalary,
            GrossSalaryFrequencyId = item.GrossSalaryFrequencyId,
            NetSalary = item.NetSalary,
            NetSalaryFrequencyId = item.NetSalaryFrequencyId,
            GrossCommissions = item.GrossCommissions,
            GrossCommissionsFrequencyId = item.GrossCommissionsFrequencyId,
            NetCommissions = item.NetCommissions,
            NetCommissionsFrequencyId = item.NetCommissionsFrequencyId,
            GrossBonus = item.GrossBonus,
            GrossBonusFrequencyId = item.GrossBonusFrequencyId,
            NetBonus = item.NetBonus,
            NetBonusFrequencyId = item.NetBonusFrequencyId
        });
    }

    private async Task SaveOtherIncome(OtherEmploymentIncomeItemModel other)
    {
        if (other.Id == 0) return;

        await Mediator.Send(new UpdateOtherIncomeCommand
        {
            Id = other.Id,
            Name = other.Name,
            Gross = other.Gross,
            GrossFrequencyId = other.GrossFrequencyId,
            Net = other.Net,
            NetFrequencyId = other.NetFrequencyId
        });
    }

    private void NavigateNext() => IncomeNavService.NavigateNext();

    }
