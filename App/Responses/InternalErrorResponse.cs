using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace EventApp.App.Responses;

public class InternalErrorResponse: IStatusResponse
{
    public int Code { get; set; } = StatusCodes.Status500InternalServerError;
    public string Message { get; set; }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}