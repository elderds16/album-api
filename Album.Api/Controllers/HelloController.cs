using Album.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Album.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        private readonly GreetingService _greetingService;
        private readonly ILogger<HelloController> _logger;

        public HelloController(GreetingService greetingService, ILogger<HelloController> logger)
        {
            _greetingService = greetingService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get(string? name)
        {
            _logger.LogInformation("Received a request with name: {Name}", name);

            var greeting = _greetingService.GetGreeting(name);

            _logger.LogInformation("Generated greeting: {Greeting}",greeting);

            return Ok(greeting);
        }
    }
}
