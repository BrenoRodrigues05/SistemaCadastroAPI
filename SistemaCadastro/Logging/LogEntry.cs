using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCadastro.Logging
{
    /// <summary>
    /// Representa uma entrada de log registrada pela aplicação no banco de dados.
    /// </summary>
    /// <remarks>
    /// Cada instância desta classe corresponde a um registro na tabela <c>ApiLogs</c>,
    /// armazenando informações detalhadas sobre requisições, erros e eventos capturados pelo sistema de logging.
    /// <para>
    /// Essa entidade é utilizada pelo <see cref="CustomLogger"/> para persistir logs automaticamente
    /// sempre que o <see cref="CustomLoggerProviderConfiguration.LogToDatabase"/> estiver habilitado.
    /// </para>
    /// </remarks>
    [Table("ApiLogs")]
    public class LogEntry
    {
        /// <summary>
        /// Identificador único do registro de log.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Data e hora em que o evento de log foi gerado (UTC).
        /// </summary>
        /// <value>
        /// O valor padrão é <see cref="DateTime.UtcNow"/>.
        /// </value>
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Nível de severidade do log.
        /// </summary>
        /// <example>
        /// Exemplos: <c>Information</c>, <c>Warning</c>, <c>Error</c>, <c>Critical</c>.
        /// </example>
        [Required]
        [MaxLength(20)]
        public string? LogLevel { get; set; }

        /// <summary>
        /// Categoria associada ao log, geralmente o nome da classe ou componente que o gerou.
        /// </summary>
        /// <example>
        /// Exemplo: <c>SistemaCadastro.Controllers.CadastroController</c>
        /// </example>
        [Required]
        [MaxLength(100)]
        public string? Category { get; set; }

        /// <summary>
        /// Mensagem principal registrada no log.
        /// </summary>
        /// <remarks>
        /// Pode conter informações sobre o evento, requisição ou status da operação.
        /// </remarks>
        public string? Message { get; set; }

        /// <summary>
        /// Texto completo da exceção associada, caso o log tenha sido gerado por um erro.
        /// </summary>
        /// <remarks>
        /// Contém o tipo da exceção, mensagem e stack trace, se disponível.
        /// </remarks>
        public string? Exception { get; set; }

        /// <summary>
        /// Caminho (endpoint) da requisição HTTP associada ao log, se aplicável.
        /// </summary>
        /// <example>
        /// Exemplo: <c>/api/cadastros/5</c>
        /// </example>
        public string? Path { get; set; }

        /// <summary>
        /// Método HTTP da requisição associada ao log, se aplicável.
        /// </summary>
        /// <example>
        /// Exemplos: <c>GET</c>, <c>POST</c>, <c>PUT</c>, <c>DELETE</c>.
        /// </example>
        public string? Method { get; set; }

        /// <summary>
        /// Código de status HTTP retornado pela resposta, se o log estiver relacionado a uma requisição.
        /// </summary>
        /// <example>
        /// Exemplos: <c>200</c>, <c>400</c>, <c>500</c>.
        /// </example>
        public int? StatusCode { get; set; }
    }
}
