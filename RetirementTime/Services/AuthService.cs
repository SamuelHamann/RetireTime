using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using RetirementTime.Models.Auth;

namespace RetirementTime.Services;

public class AuthService
{
    /// <summary>
    /// Validates the authentication state and returns the authenticated user information.
    /// Returns null if the user is not authenticated or if required claims are missing.
    /// </summary>
    public async Task<AuthenticatedUser?> GetAuthenticatedUserAsync(Task<AuthenticationState>? authenticationState)
    {
        if (authenticationState == null)
        {
            return null;
        }

        var authState = await authenticationState;
        var user = authState.User;

        if (user?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        // Extract user ID
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out long userId))
        {
            return null;
        }

        // Extract first name
        var firstNameClaim = user.FindFirst(ClaimTypes.GivenName);
        var firstName = firstNameClaim?.Value ?? string.Empty;

        // Extract email
        var emailClaim = user.FindFirst(ClaimTypes.Email);
        var email = emailClaim?.Value ?? string.Empty;

        // Extract role information
        var roleClaim = user.FindFirst(ClaimTypes.Role);
        var roleName = roleClaim?.Value ?? "User";

        var roleIdClaim = user.FindFirst("RoleId");
        var roleId = roleIdClaim != null && int.TryParse(roleIdClaim.Value, out int parsedRoleId) 
            ? parsedRoleId 
            : 1;

        return new AuthenticatedUser
        {
            UserId = userId,
            FirstName = firstName,
            Email = email,
            RoleId = roleId,
            RoleName = roleName
        };
    }
}

