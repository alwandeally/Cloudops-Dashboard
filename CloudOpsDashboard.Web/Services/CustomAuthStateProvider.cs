using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace CloudOpsDashboard.Web.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly AuthService _authService;

    public CustomAuthStateProvider(AuthService authService)
    {
        _authService = authService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var isAuthenticated = await _authService.IsAuthenticatedAsync();

        if (!isAuthenticated)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var userName = await _authService.GetUserAsync() ?? "user";
        var role = await _authService.GetRoleAsync() ?? "Viewer";

        var identity = new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, role)
            },
            "CustomAuth");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void NotifyUserAuthentication()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void NotifyUserLogout()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}