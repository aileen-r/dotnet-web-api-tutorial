using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models.Exceptions;

namespace TodoApi.Middlewares
{
  public class ExceptionHandlingMiddleware
  {
    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
      this.next = next;
      this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        await next(httpContext);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(httpContext, ex);
      }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      context.Response.ContentType = "application/json";
      var response = context.Response;

      var errorResponse = new ProblemDetails();
      switch (exception)
      {
        case TodoApiException ex:
          if (ex.Message.Contains("Invalid token"))
          {
            response.StatusCode = (int)HttpStatusCode.Forbidden;
            errorResponse.Status = (int)HttpStatusCode.Forbidden;
            errorResponse.Title = ex.Title;
            errorResponse.Detail = ex.Message;
            break;
          }
          response.StatusCode = (int)HttpStatusCode.BadRequest;
          errorResponse.Status = (int)HttpStatusCode.BadRequest;
          errorResponse.Title = ex.Title;
          errorResponse.Detail = ex.Message;
          break;
        default:
          response.StatusCode = (int)HttpStatusCode.InternalServerError;
          errorResponse.Status = (int)HttpStatusCode.InternalServerError;
          errorResponse.Detail = "Internal Server errors";
          break;
      }
      logger.LogError(exception.Message);
      var result = JsonSerializer.Serialize(errorResponse);
      await context.Response.WriteAsync(result);
    }
  }
}
