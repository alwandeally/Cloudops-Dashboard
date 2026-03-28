using System.Net.Http.Json;
using Microsoft.JSInterop;

namespace CloudOpsDashboard.Web.Services
{
    public class AuthService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _httpClient;

        public AuthService(IJSRuntime jsRuntime, HttpClient httpClient)
        {
            _jsRuntime = jsRuntime;
            _httpClient = httpClient;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var value = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "isAuthenticated");
            return bool.TryParse(value, out var result) && result;
        }

        public async Task<string?> GetUserAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authUser");
        }

        public async Task<string?> GetRoleAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authRole");
        }

        public async Task<bool> IsInRoleAsync(string role)
        {
            var currentRole = await GetRoleAsync();
            return string.Equals(currentRole, role, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            username = username?.Trim() ?? string.Empty;
            password = password?.Trim() ?? string.Empty;

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7161/api/auth/login", new
            {
                Username = username,
                Password = password
            });

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<AuthLoginResult>();

            if (result is null || !result.Success || string.IsNullOrWhiteSpace(result.Username) || string.IsNullOrWhiteSpace(result.Role))
                return false;

            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authUser", result.Username);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authRole", result.Role);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "isAuthenticated", "true");

            return true;
        }

        public async Task LogoutAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authUser");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authRole");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "isAuthenticated");
        }

        private class AuthLoginResult
        {
            public bool Success { get; set; }
            public string? Username { get; set; }
            public string? Role { get; set; }
            public string? Message { get; set; }
        }
    }
}