using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TasksManager.Services.Exceptions;

namespace TasksManager.Web.Infrastructure
{
    public class ExceptionMiddleware
    {
        private const string DefaultError = "Unexpected server error";
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            logger = loggerFactory?.CreateLogger<ExceptionMiddleware>() ??
                     throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (TaskNotFoundException ex)
            {
                logger.LogWarning(ex.Message);
                await HandleExceptionAsync(context, HttpStatusCode.NotFound);
            }
            catch (TaskProcessingException ex)
            {
                logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, message: ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during request processing");
                await HandleExceptionAsync(context);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode code = HttpStatusCode.InternalServerError, string message = DefaultError)
        {
            var result = JsonConvert.SerializeObject(new {message, code});
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            return context.Response.WriteAsync(result);
        }
    }
}