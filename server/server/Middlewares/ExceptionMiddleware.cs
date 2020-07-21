using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Server.Helpers;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Server.Middlewares
{
  public class ExceptionMiddleware
  {
    private readonly RequestDelegate _next;
    protected readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
      _next = next;
      _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        _logger.LogError(StringHelper.AplicationException + ex.Message + ex.StackTrace);
        await HandleExceptionAsync(context, ex);
      }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
      var result = JsonConvert.SerializeObject(new { error = StringHelper.InternalServerError });
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
      return context.Response.WriteAsync(result);
    }
  }
}
