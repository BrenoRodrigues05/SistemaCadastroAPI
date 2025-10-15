using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using SistemaCadastro.Models;
using System.Net;
using System.Text.Json;

namespace SistemaCadastro.Extensions
{
    /// <summary>
    /// Extensões para configurar tratamento global de exceções na API.
    /// </summary>
    public static class ApiExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Configura o middleware para captura de exceções não tratadas e retorno de JSON padronizado.
        /// </summary>
        /// <param name="app">Aplicativo ASP.NET Core.</param>
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    // Define status HTTP 500 e tipo de conteúdo JSON
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    // Recupera a exceção capturada pelo middleware
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var errorDetails = new ErrorDetails
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            Trace = contextFeature.Error.StackTrace
                        };

                        // Serializa e envia a resposta JSON
                        await context.Response.WriteAsync(errorDetails.ToString());
                    }
                });
            });
        }
    }
}
