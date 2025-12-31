using Microsoft.AspNetCore.Authentication.Cookies;
using Radzen;
using Template.Portal.Components;
using Template.Portal.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddCustomScopedServices(builder);

builder.Services.AddCustomAuthorizationPolicies();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = ".Template.Auth";
        options.Cookie.Path = "/";
        options.LoginPath = "/account/login";
        options.LogoutPath = "/account/logout";
        options.AccessDeniedPath = "/access-denied";

        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

builder.Services.AddAuthorization();
// Register Radzen.Blazor services
builder.Services.AddRadzenComponents();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found");

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorPages();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();