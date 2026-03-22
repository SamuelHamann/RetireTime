using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Users.CompleteIntro;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard;

public partial class DashboardSettings
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    private async Task ResetIntro()
    {
        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);

        if (authenticatedUser != null)
        {
            await Mediator.Send(new CompleteIntroCommand
            {
                UserId = authenticatedUser.UserId,
                HasCompletedIntro = false
            });
        }

        Navigation.NavigateTo("/introduction");
    }
}

