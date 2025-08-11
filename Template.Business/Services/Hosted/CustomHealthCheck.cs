using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Business.Services.Hosted
{
    public class CustomHealthCheck : IHealthCheck
    {
        private Random _random = new Random();
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var responseTime = _random.Next(1, 300);

            return Task.FromResult(
                responseTime switch
                {
                    < 100 => HealthCheckResult.Healthy("Healthy"),
                    (> 100) and (< 200) => HealthCheckResult.Degraded("Degraded"),
                    _ => HealthCheckResult.Unhealthy("Unhealthy")
                });
        }
    }
}
