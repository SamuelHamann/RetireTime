using Microsoft.AspNetCore.Components;
using RetirementTime.Services;

namespace RetirementTime.Components;

public class AuthenticatedComponentBase : ComponentBase
{
    [Inject] protected AuthService AuthService { get; set; } = null!;
    [Inject] protected NavigationManager Navigation { get; set; } = null!;

    private bool _hasCheckedAuth = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !_hasCheckedAuth)
        {
            _hasCheckedAuth = true;
            AuthService.Initialize();
            var isAuthenticated = await AuthService.IsAuthenticated();
            
            if (!isAuthenticated)
            {
                Navigation.NavigateTo("/login", true);
            }
        }
        
        await base.OnAfterRenderAsync(firstRender);
    }
}

