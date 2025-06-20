using Album.Api.Models;
using System.Net;

namespace Album.Api.Services;

public class GreetingService
{
    public GreetingResponse GetGreeting(string? name)
    {
        var hostname = Dns.GetHostName();
        var message = string.IsNullOrWhiteSpace(name) ? $"Hello World from {hostname} v2" : $"Hello {name} from {hostname} v2";
        return new GreetingResponse(message);
    }
}
