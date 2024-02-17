using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Mango.Services.CouponAPI
{
    public static class Extensions
    {
        public static WebApplication MapDefaultEndpoints(this WebApplication app)
        {
            // Uncomment the following line to enable the Prometheus endpoint (requires the OpenTelemetry.Exporter.Prometheus.AspNetCore package)
            // app.MapPrometheusScrapingEndpoint();

            // All health checks must pass for app to be considered ready to accept traffic after starting
            app.MapHealthChecks("/health");

            // Only health checks tagged with the "live" tag must pass for app to be considered alive
            app.MapHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live")
            });

            return app;
        }
    }
}
