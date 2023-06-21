using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace EventApp.App.Responses;

public class NotFoundResponse : IStatusResponse
{
    public int Code { get; set; } = StatusCodes.Status404NotFound;
    public string Message { get; set; }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}