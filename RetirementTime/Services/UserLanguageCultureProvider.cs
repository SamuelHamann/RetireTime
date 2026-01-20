using Microsoft.AspNetCore.Localization;
using MediatR;
using RetirementTime.Application.Features.Users.ValidateSession;
using RetirementTime.Application.Features.Users.GetUserById;

namespace RetirementTime.Services;

public class UserLanguageCultureProvider : RequestCultureProvider
{
    public override async Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        // Try to get the mediator from DI
        var mediator = httpContext.RequestServices.GetService<IMediator>();
        
        if (mediator == null)
        {
            return new ProviderCultureResult("en"); // default to English
        }

        try
        {
            // Get session token from cookie
            var sessionToken = httpContext.Request.Cookies["RetireTime_Session"];
            
            if (string.IsNullOrEmpty(sessionToken))
            {
                return new ProviderCultureResult("en"); // default to English for non-authenticated users
            }

            // Validate session and get user info
            var sessionQuery = new ValidateSessionQuery
            {
                SessionToken = sessionToken
            };
            
            var sessionResult = await mediator.Send(sessionQuery);
            
            if (!sessionResult.IsValid || sessionResult.UserId == null)
            {
                return new ProviderCultureResult("en"); // default to English
            }

            // Get user's language preference
            var getUserQuery = new GetUserByIdQuery
            {
                UserId = sessionResult.UserId.Value
            };
            
            var user = await mediator.Send(getUserQuery);
            
            if (user != null && !string.IsNullOrEmpty(user.LanguageCode))
            {
                return new ProviderCultureResult(user.LanguageCode);
            }
        }
        catch
        {
            // If anything fails, default to English
        }

        return new ProviderCultureResult("en"); // default to English
    }
}

