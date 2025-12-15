using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;
using Template.Library.Extensions;
using Template.Library.Models;
using Template.Portal.Components;
using Template.Portal.Extensions;
using Template.Portal.Interface;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddCustomScopedServices(builder);
builder.Services.AddHttpClient();
builder.Services.AddScoped(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    var client = new HttpClient
    {
        BaseAddress = new Uri(navigationManager.BaseUri)
    };
    return client;
});
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.Name = ".AspNetCore.Antiforgery";
    options.Cookie.HttpOnly = true;
    //options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    //options.Cookie.SameSite = SameSiteMode.Strict;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = ".Template.Auth";
        options.LoginPath = "/account/login";
        options.LogoutPath = "/account/logout";
        options.AccessDeniedPath = "/access-denied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        //options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Use Always in production
        options.Cookie.SecurePolicy = CookieSecurePolicy.None; // TEMPORARY FOR DEBUGGING - change to Always in production
        options.Cookie.SameSite = SameSiteMode.None; // Important for cross-origin if needed
        options.Cookie.IsEssential = true;

        // Add events to debug
        options.Events = new CookieAuthenticationEvents
        {
            OnSigningIn = context =>
            {
                Console.WriteLine($"Setting auth cookie for {context.Principal.Identity.Name}");
                Console.WriteLine($"Setting cookie: {options.Cookie.Name}");
                return Task.CompletedTask;
            },
            OnSignedIn = context =>
            {
                Console.WriteLine($"Auth cookie set for {context.Principal.Identity.Name}");
                return Task.CompletedTask;
            }
        };
    });

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://localhost:7098", "http://localhost:5039")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // THIS IS CRITICAL FOR COOKIES
        });
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
  
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
//app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();


app.MapPost("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    context.Response.Redirect("/account/login");

}).DisableAntiforgery();

app.MapPost("/login", async (HttpContext context, [FromBody] LoginRequest request, [FromServices] IPortalService portalService) =>
{
    var user = await portalService.Account.LoginUserUserAsync(model: new RequestLoginAccount
    {
        Email = request.Email,
        Password = request.Password,
    });

    var claims = user.GetUserClaims();
    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var principal = new ClaimsPrincipal(identity);

    await context.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        principal,
        new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
            AllowRefresh = true,
            IssuedUtc = DateTimeOffset.UtcNow,

            // Explicit cookie options
            Items =
            {
                { ".issued", DateTimeOffset.UtcNow.ToString("o") },
                { ".expires", DateTimeOffset.UtcNow.AddDays(7).ToString("o") }
            }
        });

    // DEBUG: Check what cookies are being set
    context.Response.Headers.Append("X-Debug-Cookies", string.Join(", ", context.Response.Headers["Set-Cookie"]));

    return Results.Ok();
    //return Results.Ok(new { success = true, redirectUrl = "/" });

});

app.Run();
