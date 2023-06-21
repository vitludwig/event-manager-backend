using EventApp.App.Exceptions;
using EventApp.App.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EventApp.App.Middleware;

public class RuntimeExceptionMiddleware
{
  private readonly RequestDelegate _next;

  public RuntimeExceptionMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext httpContext)
  {
    try
    {
      await _next(httpContext);
    }
    catch (BadRequestException ex)
    {
      await HandleExceptionAsync(httpContext, ex, HttpStatusCode.BadRequest);
    }
    catch (InvalidReferenceException ex)
    {
      await HandleExceptionAsync(httpContext, ex, HttpStatusCode.BadRequest);
    }
    catch (NotFoundException ex)
    {
      await HandleExceptionAsync(httpContext, ex, HttpStatusCode.NotFound);
    }
    catch (ForbiddenException ex)
    {
      await HandleExceptionAsync(httpContext, ex, HttpStatusCode.Forbidden);
    }
    catch (ConflictException ex)
    {
      await HandleExceptionAsync(httpContext, ex, HttpStatusCode.Conflict);
    }
    catch (Exception)
    {
      httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
      throw;
    }
    // catch (OperationFailedException ex)
    // {
    //     // _logger.Log(LogLevel.Error, ex, $"Exception: {ex.Message}");
    //     await HandleExceptionAsync(httpContext, ex, HttpStatusCode.BadRequest);
    // }
    // catch (ArgumentException ex)
    // {
    //     // _logger.Log(LogLevel.Error, ex, $"Exception: {ex.Message}");
    //     await HandleExceptionAsync(httpContext, ex, HttpStatusCode.BadRequest);
    // }
  }


  private static async Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
  {
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)statusCode;

    IStatusResponse statusResponse = statusCode switch
    {
      HttpStatusCode.BadRequest => new BadRequestResponse() { Message = exception.Message },
      HttpStatusCode.NotFound => new NotFoundResponse() { Message = exception.Message },
      HttpStatusCode.Forbidden => new ForbiddenResponse() { Message = exception.Message },
      HttpStatusCode.Conflict => new ForbiddenResponse() { Message = exception.Message },
      _ => new InternalErrorResponse() { Message = "Unexpected error has happened." }
    };

    await context.Response.WriteAsync(statusResponse.ToString() ?? string.Empty);
  }
}
