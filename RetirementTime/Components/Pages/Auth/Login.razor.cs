using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Users.Login;
using RetirementTime.Models.Auth;
using RetirementTime.Resources.Auth;

namespace RetirementTime.Components.Pages.Auth;

public partial class Login
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private IStringLocalizer<AuthResources> Localizer { get; set; } = default!;

    private LoginModel LoginModel { get; set; } = new();
    private string _errorMessage = string.Empty;

    private async Task HandleLogin()
    {
        _errorMessage = string.Empty;

        var command = new LoginCommand
        {
            Email = LoginModel.Email,
            Password = LoginModel.Password
        };

        var result = await Mediator.Send(command);

        if (result is { Success: true, UserId: not null })
        {
            var queryString = $"?userId={result.UserId}" +
                              $"&firstName={Uri.EscapeDataString(result.FirstName ?? string.Empty)}" +
                              $"&roleId={result.RoleId ?? 1}" +
                              $"&roleName={Uri.EscapeDataString(result.RoleName ?? "User")}";

            Navigation.NavigateTo($"/api/auth/login{queryString}", forceLoad: true);
        }
        else
        {
            _errorMessage = result.ErrorMessage ?? Localizer["Login_ErrorGeneric"];
        }
    }

    private async Task HandleGoogleLogin()
    {
        // TODO: Implement Google OAuth login
        await Task.CompletedTask;
    }
}



