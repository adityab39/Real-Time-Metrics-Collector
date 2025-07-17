using Microsoft.AspNetCore.Mvc;
using RealTimeMetricsApi.Models;
using RealTimeMetricsApi.Services;

namespace RealTimeMetricsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetricsController : ControllerBase
    {
        private readonly MetricService _service;

        public MetricsController(MetricService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult PostMetric([FromBody] Metric metric)
        {
            var result = _service.CheckAndSaveMetric(metric);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetMetricsByAppId([FromQuery] string appId)
        {
            if (string.IsNullOrWhiteSpace(appId))
                return BadRequest("The 'appId' query parameter is required.");

            var metrics = _service.GetMetricsByAppId(appId);

            if (metrics.Count == 0)
                return NotFound($"No metrics found for appId: {appId}");

            return Ok(metrics);
        }
    }
}