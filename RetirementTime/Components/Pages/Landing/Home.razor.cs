using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace RetirementTime.Components.Pages.Landing;

public partial class Home
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = default!;
    
    private string _currentLanguageDisplay = "EN";

    protected override void OnInitialized()
    {
        // Get current culture to display the toggle button correctly
        var currentCulture = CultureInfo.CurrentUICulture.Name;
        _currentLanguageDisplay = currentCulture.StartsWith("fr") ? "FR" : "EN";
    }

    private async Task ToggleLanguage()
    {
        var currentCulture = CultureInfo.CurrentUICulture.Name;
        var newCulture = currentCulture.StartsWith("fr") ? "en-CA" : "fr-CA";
        
        // Store in local storage
        await JsRuntime.InvokeVoidAsync("localStorage.setItem", "preferredLanguage", newCulture);
        
        // Set cookie for culture
        await JsRuntime.InvokeVoidAsync("eval", $"document.cookie = '.AspNetCore.Culture=c={newCulture}|uic={newCulture}; path=/; max-age=31536000'");
        
        // Reload the page to apply the new culture
        Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
    }

    private void HandleLoginClick()
    {
        Navigation.NavigateTo("/login");
    }
}

