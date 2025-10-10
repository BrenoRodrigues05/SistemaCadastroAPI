using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace SistemaCadastro.Filters
{
    public class APILoggingFilter : IActionFilter
    {
        private readonly ILogger<APILoggingFilter> _logger;
        private readonly Stopwatch _stopwatch = new();

        public APILoggingFilter(ILogger<APILoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch.Start();

            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            var method = context.HttpContext.Request.Method;
            var path = context.HttpContext.Request.Path;

            string? body = null;
            if (context.ActionArguments.Any())
            {
                // Serializa parâmetros recebidos (DTOs, IDs, etc.)
                body = JsonSerializer.Serialize(context.ActionArguments, new JsonSerializerOptions
                {
                    WriteIndented = false,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });
            }

            _logger.LogInformation(
                "➡️ Requisição iniciada: {Method} {Path} | Controller: {Controller} | Action: {Action} | Dados: {Body}",
                method, path, controller, action, body ?? "Sem corpo"
            );
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();

            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            var method = context.HttpContext.Request.Method;
            var path = context.HttpContext.Request.Path;
            var statusCode = context.HttpContext.Response?.StatusCode;

            if (context.Exception != null)
            {
                _logger.LogError(context.Exception,
                    "❌ Erro durante a execução: {Method} {Path} | Controller: {Controller} | Action: {Action} | Tempo: {Elapsed}ms",
                    method, path, controller, action, _stopwatch.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogInformation(
                    "✅ Requisição concluída: {Method} {Path} | Controller: {Controller} | Action: {Action} | Status: {StatusCode} | Tempo: {Elapsed}ms",
                    method, path, controller, action, statusCode, _stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
