using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace CycleFinder.Middlewares
{
    public class HttpRequestResponseLogger
    {
        private readonly ILogger<HttpRequestResponseLogger> _logger;
        private readonly RequestDelegate _next;

        //TODO: add a switch to control response logging from appsettings
        public HttpRequestResponseLogger(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpRequestResponseLogger>(); ;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation(
                    "Request {method} {url}{query}",
                    context.Request?.Method,
                    context.Request?.Path.Value,
                    context.Request.QueryString.ToString());

            var originalBody = context.Response.Body;
            using var newBody = new MemoryStream();
            context.Response.Body = newBody;

            try
            {
                await _next(context);
            }
            finally
            {
                newBody.Seek(0, SeekOrigin.Begin);
                var bodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                _logger.LogInformation($"LoggingMiddleware: {bodyText}");
                newBody.Seek(0, SeekOrigin.Begin);
                await newBody.CopyToAsync(originalBody);
            }
        }
    }
}
