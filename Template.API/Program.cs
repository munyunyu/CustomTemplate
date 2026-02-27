using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Template.API.Extensions;
using Template.Business.Services.Hosted;
using Template.Database.Context;
using Template.Library.Mapper;
using Template.Library.Views;
using Template.Service.Extensions;
using Template.Service.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSwaggerGen();

builder.Services.AddCustomIdentity();

builder.Services.AddCustomAuthentication(builder);

builder.Services.AddCustomScopedServices(builder);

builder.Services.AddCustomHealthChecks(builder);

builder.Services.AddCustomAuthorizationPolicies();

builder.AddCustomSeriLogging();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

builder.Services.AddCorsConfiguration(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();

    await app.Services.SeedAsync(); // Seed roles & claims

    await app.Services.SeedViewsAsync(); //seed views
}

app.UseHttpsRedirection();

app.UseCors("FrontEnd");

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

