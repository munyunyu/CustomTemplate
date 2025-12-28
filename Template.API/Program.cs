using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Template.API.Extensions;
using Template.Business.Services.Hosted;
using Template.Database.Context;
using Template.Library.Mapper;
using Template.Service.Extensions;

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.Services.SeedAsync(); // Seed roles & claims
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

