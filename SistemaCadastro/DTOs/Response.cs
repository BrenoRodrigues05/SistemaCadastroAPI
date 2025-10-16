namespace SistemaCadastro.DTOs
{
    /// <summary>
    /// Representa uma resposta genérica de uma operação.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Status da operação (por exemplo, "Success", "Error").
        /// </summary>
        /// <example>Success</example>
        public string? Status { get; set; }

        /// <summary>
        /// Mensagem descritiva da operação.
        /// </summary>
        /// <example>Operação realizada com sucesso.</example>
        public string? Message { get; set; }
    }
}
