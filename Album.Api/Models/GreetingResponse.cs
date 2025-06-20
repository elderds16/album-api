namespace Album.Api.Models;

public class GreetingResponse
{
    public GreetingResponse(string message)
    {
        Message = message;
    }

    public string Message { get; set; }

}

