namespace Template.Service.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "FrontEnd",
                                  policy =>
                                  {
                                      policy
                                     // .WithOrigins("http://localhost:4200")
                                     .SetIsOriginAllowed(origin => true)
                                      .AllowAnyMethod()
                                      .AllowCredentials()
                                      .AllowAnyHeader();

                                  });
            });
            return services;
        }
    }
}
