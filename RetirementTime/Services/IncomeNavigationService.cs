namespace RetirementTime.Services;

public class IncomeNavigationService
{
    public event Action? OnNavigateNext;

    public void NavigateNext() => OnNavigateNext?.Invoke();

    public void Register(Action handler) => OnNavigateNext += handler;

    public void Unregister(Action handler) => OnNavigateNext -= handler;
}

