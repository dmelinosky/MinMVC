using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ClientResourceFullCS.Controllers
{
    public class HealthCheckController : Controller
    {
        public HealthCheckController(HealthCheckService service)
        {
            this.Service = service;
        }

        public HealthCheckService Service { get; }


        // GET: HealthCheck
        public async Task<ActionResult> Index()
        {
            var report = await this.Service.CheckHealthAsync();

            return this.Json(report, JsonRequestBehavior.AllowGet);
        }
    }

    public class ExampleHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var healthCheckResultHealthy = true;

            if (healthCheckResultHealthy)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("A healthy result."));
            }

            return Task.FromResult(
                HealthCheckResult.Unhealthy("An unhealthy result."));
        }
    }
}