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
        public IActionResult GetMetrics(
        [FromQuery] string appId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
        {
            if (string.IsNullOrWhiteSpace(appId))
                return BadRequest("The 'appId' query parameter is required.");

            var metrics = _service.GetMetrics(appId, startDate, endDate);

            if (metrics.Count == 0)
                return NotFound("No metrics found for given parameters.");

            return Ok(metrics);
        }

        [HttpGet("stats")]
        public IActionResult GetStats(
            [FromQuery] string appId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            if (string.IsNullOrWhiteSpace(appId))
                return BadRequest("The 'appId' query parameter is required.");

            var stats = _service.GetStats(appId, startDate, endDate);
            if (stats is null)
                return NotFound("No metrics found for the given parameters.");

            return Ok(stats);
        }

        [HttpGet("export")]
        public IActionResult ExportMetrics(
            [FromQuery] string appId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            if (string.IsNullOrWhiteSpace(appId))
                return BadRequest("The 'appId' query parameter is required.");

            var csvBytes = _service.ExportMetricsToCsv(appId, startDate, endDate);
            if (csvBytes == null || csvBytes.Length == 0)
                return NotFound("No data to export.");

            return File(csvBytes, "text/csv", $"{appId}_metrics.csv");
        }
    }
}