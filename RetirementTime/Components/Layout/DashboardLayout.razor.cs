using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RetirementTime.Application.Features.Users.GetIntroStatus;
using RetirementTime.Services;

namespace RetirementTime.Components.Layout;

public partial class DashboardLayout : LayoutComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    internal bool HasCompletedIntro { get; private set; } = true;
    internal string _initials = "?";

    protected override async Task OnInitializedAsync()
    {
        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) return;

        if (!string.IsNullOrEmpty(authenticatedUser.FirstName))
            _initials = authenticatedUser.FirstName.Length >= 2 ? authenticatedUser.FirstName[..2].ToUpper() : authenticatedUser.FirstName.ToUpper();
        else if (!string.IsNullOrEmpty(authenticatedUser.Email))
            _initials = authenticatedUser.Email.Length >= 2 ? authenticatedUser.Email[..2].ToUpper() : authenticatedUser.Email.ToUpper();

        var result = await Mediator.Send(new GetIntroStatusQuery { UserId = authenticatedUser.UserId });

        HasCompletedIntro = result.HasCompletedIntro;

        if (!HasCompletedIntro)
        {
            Navigation.NavigateTo("/introduction", replace: true);
        }
    }
}
