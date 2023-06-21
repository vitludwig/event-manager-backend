using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace EventApp.App.Responses;

public class BadRequestResponse : IStatusResponse
{
    public int Code { get; set; } = StatusCodes.Status400BadRequest;
    public string Message { get; set; }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}