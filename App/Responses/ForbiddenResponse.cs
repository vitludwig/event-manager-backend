using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace EventApp.App.Responses;

public class ForbiddenResponse : IStatusResponse
{
    public int Code { get; set; } = StatusCodes.Status403Forbidden;
    public string Message { get; set; }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}