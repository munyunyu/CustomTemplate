namespace Template.Service.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetSection("AllowedCorsOrigins").Get<string[]>() ?? [];

            services.AddCors(options =>
            {
                options.AddPolicy(name: "FrontEnd",
                                  policy =>
                                  {
                                      policy
                                      .WithOrigins(allowedOrigins)
                                      .AllowAnyMethod()
                                      .AllowCredentials()
                                      .AllowAnyHeader();
                                  });
            });
            return services;
        }
    }
}
