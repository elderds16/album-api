using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Album.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("GET /api/test called");
            return Ok("TestController is alive!");
        }

        [HttpGet("time")]
        public IActionResult GetServerTime()
        {
            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var message = $"Test -- The date now is: {now}";

            _logger.LogInformation("GET /api/test/time called, returning message: {Message}", message);

            return Ok(message);
        }

    }
}
