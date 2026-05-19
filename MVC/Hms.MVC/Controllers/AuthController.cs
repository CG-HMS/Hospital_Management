using Hms.MVC.Models.Auth;
using Hms.MVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace Hms.MVC.Controllers;

public class AuthController : Controller
{
    private readonly IApiService _api;

    public AuthController(IApiService api)
    {
        _api = api;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        // If already authenticated, redirect to dashboard
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Dashboard");

        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var response = await _api.PostRawAsync("auth/login", new
            {
                email = model.Email,
                password = model.Password
            });

            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                string errorMsg = "Invalid email or password.";

                try
                {
                    var errorObj = JsonSerializer.Deserialize<JsonElement>(errorJson);
                    if (errorObj.TryGetProperty("message", out var msg))
                        errorMsg = msg.GetString() ?? errorMsg;
                }
                catch { }

                model.ErrorMessage = errorMsg;
                return View(model);
            }

            var json = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<LoginApiResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (loginResponse == null)
            {
                model.ErrorMessage = "Unexpected response from server.";
                return View(model);
            }

            // Store JWT in session
            HttpContext.Session.SetString("JwtToken", loginResponse.Token);
            HttpContext.Session.SetString("Username", loginResponse.Username);
            HttpContext.Session.SetString("UserRole", loginResponse.Role);

            // Parse JWT to get claims for cookie auth
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(loginResponse.Token);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, loginResponse.Username),
                new(ClaimTypes.Role, loginResponse.Role),
                new(ClaimTypes.Email, model.Email)
            };

            // Copy refId if present
            var refIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == "refId");
            if (refIdClaim != null)
            {
                claims.Add(new Claim("refId", refIdClaim.Value));
                HttpContext.Session.SetString("RefId", refIdClaim.Value);
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = loginResponse.ExpiresAt
                });

            TempData["SuccessMessage"] = $"Welcome back, {loginResponse.Username}!";

            if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            return RedirectToAction("Index", "Dashboard");
        }
        catch (Exception)
        {
            model.ErrorMessage = "Unable to connect to the server. Please try again.";
            return View(model);
        }
    }

    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["SuccessMessage"] = "You have been logged out successfully.";
        return RedirectToAction("Login");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Users()
    {
        var users = await _api.GetAsync<List<UserViewModel>>("auth/users");
        return View(users?.OrderBy(u => u.UserId).ToList() ?? new List<UserViewModel>());
    }

    // ── GET /Auth/CreateUser  — ADMIN ────────────────────────────────────────
    [Authorize(Roles = "admin")]
    [HttpGet]
    public IActionResult CreateUser()
    {
        return View(new CreateUserViewModel());
    }

    // ── POST /Auth/CreateUser  — ADMIN ───────────────────────────────────────
    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var payload = new
            {
                username = model.Username,
                email    = model.Email,
                password = model.Password,   // API hashes with SHA-256 internally
                role     = model.Role,
                refId    = model.RefId
            };

            var response = await _api.PostRawAsync("auth/users", payload);

            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                string errorMsg = "Failed to create user.";

                try
                {
                    var errorObj = JsonSerializer.Deserialize<JsonElement>(errorJson);
                    if (errorObj.TryGetProperty("message", out var msg))
                        errorMsg = msg.GetString() ?? errorMsg;
                }
                catch { }

                model.ErrorMessage = errorMsg;
                return View(model);
            }

            TempData["SuccessMessage"] = $"User '{model.Username}' created successfully.";
            return RedirectToAction(nameof(Users));
        }
        catch (Exception)
        {
            model.ErrorMessage = "Unable to connect to the server. Please try again.";
            return View(model);
        }
    }
}
