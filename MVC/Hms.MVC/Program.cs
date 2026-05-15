using Hms.MVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Hms.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ── MVC + Razor ──────────────────────────────────────────────────
            builder.Services.AddControllersWithViews();

            // ── Session ──────────────────────────────────────────────────────
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = ".HMS.Session";
            });

            // ── Cookie Authentication ────────────────────────────────────────
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.LogoutPath = "/Auth/Logout";
                    options.AccessDeniedPath = "/Auth/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.SlidingExpiration = true;
                });

            // ── HttpClient for API calls ─────────────────────────────────────
            var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]
                             ?? "https://localhost:7001/api/";

            builder.Services.AddHttpClient("HmsApi", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            });

            // ── Custom Services ──────────────────────────────────────────────
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IApiService, ApiService>();

            var app = builder.Build();

            // ── Middleware Pipeline ───────────────────────────────────────────
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
