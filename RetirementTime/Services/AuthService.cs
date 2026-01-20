using MediatR;
using Microsoft.JSInterop;

namespace RetirementTime.Services;

public partial class AuthService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IMediator _mediator;
    private readonly ILogger<AuthService> _logger;
    private const string SessionCookieName = "RetireTime_Session";
    private bool _isInitialized;

    public AuthService(IJSRuntime jsRuntime, IMediator mediator, ILogger<AuthService> logger)
    {
        _jsRuntime = jsRuntime;
        _mediator = mediator;
        _logger = logger;
    }

    public void Initialize()
    {
        _isInitialized = true;
    }

    public async Task<bool> IsAuthenticated()
    {
        LogCheckingAuthentication(_logger);
        
        try
        {
            var sessionToken = await GetSessionToken();
            if (string.IsNullOrEmpty(sessionToken))
            {
                LogNoSessionTokenFound(_logger);
                return false;
            }

            // Validate session with the application layer
            var query = new Application.Features.Users.ValidateSession.ValidateSessionQuery
            {
                SessionToken = sessionToken
            };

            var result = await _mediator.Send(query);
            
            if (result.IsValid)
            {
                LogAuthenticationSuccessful(_logger, result.UserId ?? 0);
            }
            else
            {
                LogAuthenticationFailed(_logger);
            }
            
            return result.IsValid;
        }
        catch (Exception ex)
        {
            LogAuthenticationError(_logger, ex.Message);
            return false;
        }
    }

    public async Task SetSessionToken(string sessionToken)
    {
        LogSettingSessionToken(_logger, sessionToken);
        await _jsRuntime.InvokeVoidAsync("RetireTimeCookies.setCookie", 
            SessionCookieName, sessionToken, 1800);
    }

    private async Task<string?> GetSessionToken()
    {
        if (!_isInitialized)
        {
            LogJsInteropNotAvailable(_logger);
            return null;
        }

        try
        {
            var cookie = await _jsRuntime.InvokeAsync<string?>("RetireTimeCookies.getCookie", 
                SessionCookieName);
            
            if (!string.IsNullOrEmpty(cookie))
            {
                LogSessionTokenRetrieved(_logger, cookie);
            }
            
            return cookie;
        }
        catch (Exception ex)
        {
            LogErrorRetrievingSessionToken(_logger, ex.Message);
            return null;
        }
    }

    public async Task ClearSession()
    {
        LogClearingSession(_logger);
        await _jsRuntime.InvokeVoidAsync("RetireTimeCookies.deleteCookie", 
            SessionCookieName);
    }

    [LoggerMessage(LogLevel.Information, "Checking authentication status")]
    static partial void LogCheckingAuthentication(ILogger<AuthService> logger);

    [LoggerMessage(LogLevel.Debug, "JavaScript interop not available (prerendering)")]
    static partial void LogJsInteropNotAvailable(ILogger<AuthService> logger);

    [LoggerMessage(LogLevel.Warning, "No session token found in cookie")]
    static partial void LogNoSessionTokenFound(ILogger<AuthService> logger);

    [LoggerMessage(LogLevel.Information, "Authentication successful for UserId: {userId}")]
    static partial void LogAuthenticationSuccessful(ILogger<AuthService> logger, long userId);

    [LoggerMessage(LogLevel.Warning, "Authentication failed - invalid or expired session")]
    static partial void LogAuthenticationFailed(ILogger<AuthService> logger);

    [LoggerMessage(LogLevel.Error, "Error during authentication check | Exception: {exception}")]
    static partial void LogAuthenticationError(ILogger<AuthService> logger, string exception);

    [LoggerMessage(LogLevel.Information, "Setting session token: {sessionToken}")]
    static partial void LogSettingSessionToken(ILogger<AuthService> logger, string sessionToken);

    [LoggerMessage(LogLevel.Information, "Session token retrieved from cookie: {sessionToken}")]
    static partial void LogSessionTokenRetrieved(ILogger<AuthService> logger, string sessionToken);

    [LoggerMessage(LogLevel.Error, "Error retrieving session token from cookie | Exception: {exception}")]
    static partial void LogErrorRetrievingSessionToken(ILogger<AuthService> logger, string exception);

    [LoggerMessage(LogLevel.Information, "Clearing session cookie")]
    static partial void LogClearingSession(ILogger<AuthService> logger);
}
