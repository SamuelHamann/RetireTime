using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace RetirementTime.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpGet("login")]
    public async Task<IActionResult> Login(
        [FromQuery] long userId,
        [FromQuery] string? firstName,
        [FromQuery] int? roleId,
        [FromQuery] string? roleName,
        [FromQuery] bool hasCompletedIntro = false)
    {
        if (userId <= 0)
        {
            return BadRequest("Invalid user ID");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.GivenName, firstName ?? string.Empty),
            new Claim(ClaimTypes.Role, roleName ?? "User"),
            new Claim("RoleId", (roleId ?? 1).ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        return Redirect(hasCompletedIntro ? "/home" : "/introduction");
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }
}



