using System.Text.Json;

namespace SistemaCadastro.Models
{
    /// <summary>
    /// Representa os detalhes de um erro retornado pela API.
    /// </summary>
    /// <remarks>
    /// Esta classe é usada pelo middleware de tratamento global de exceções
    /// para padronizar a resposta JSON em caso de erro.
    /// </remarks>
    public class ErrorDetails
    {
        /// <summary>
        /// Código HTTP do erro.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Mensagem de erro detalhando o problema.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Stack trace da exceção (opcional, útil para debug).
        /// </summary>
        public string? Trace { get; set; }

        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public ErrorDetails() { }

        /// <summary>
        /// Converte o objeto para JSON.
        /// </summary>
        /// <returns>String JSON representando o erro.</returns>
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
