using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace SistemaCadastro.Filters
{
    /// <summary>
    /// Filtro de ação responsável por registrar logs detalhados de requisições e respostas da API.
    /// </summary>
    /// <remarks>
    /// Este filtro implementa a interface <see cref="IActionFilter"/> e atua antes e depois da execução das ações dos controllers.
    /// Ele registra:
    /// <list type="bullet">
    /// <item><description>O método HTTP e o caminho da requisição.</description></item>
    /// <item><description>O nome do controller e da action executada.</description></item>
    /// <item><description>Os dados de entrada (DTOs, parâmetros, IDs, etc.).</description></item>
    /// <item><description>O status da resposta, tempo total de execução e exceções (caso ocorram).</description></item>
    /// </list>
    /// O objetivo é facilitar auditoria, monitoramento de desempenho e depuração.
    /// </remarks>
    public class APILoggingFilter : IActionFilter
    {
        private readonly ILogger<APILoggingFilter> _logger;
        private readonly Stopwatch _stopwatch = new();

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="APILoggingFilter"/>.
        /// </summary>
        /// <param name="logger">Instância de <see cref="ILogger"/> injetada via Dependency Injection para registrar os logs.</param>
        public APILoggingFilter(ILogger<APILoggingFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Executado antes da ação do controller ser invocada.
        /// </summary>
        /// <param name="context">Contexto da execução, contendo informações sobre a requisição atual, como controller, action e parâmetros.</param>
        /// <remarks>
        /// Este método registra informações da requisição recebida, incluindo:
        /// <list type="bullet">
        /// <item><description>Método HTTP e rota.</description></item>
        /// <item><description>Controller e Action.</description></item>
        /// <item><description>Corpo da requisição (parâmetros e DTOs).</description></item>
        /// </list>
        /// </remarks>
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

        /// <summary>
        /// Executado após a execução da ação do controller.
        /// </summary>
        /// <param name="context">Contexto da execução, contendo informações sobre a resposta, exceções e status HTTP.</param>
        /// <remarks>
        /// Este método registra o status final da resposta, tempo de execução e possíveis exceções.  
        /// Ele diferencia logs de sucesso (<c>LogInformation</c>) e logs de erro (<c>LogError</c>).
        /// </remarks>
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
