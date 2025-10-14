using Microsoft.Extensions.Logging;

namespace SistemaCadastro.Logging
{
    /// <summary>
    /// Define as configurações para o <see cref="CustomLoggerProvider"/>, 
    /// controlando o comportamento e os destinos de gravação de logs do sistema.
    /// </summary>
    /// <remarks>
    /// Esta classe é utilizada para parametrizar o <see cref="CustomLoggerProvider"/>, 
    /// permitindo definir o nível mínimo de log, o caminho do arquivo de saída e 
    /// se os logs devem ser gravados em arquivo, banco de dados ou ambos.
    /// <para>
    /// As configurações podem ser aplicadas via injeção de dependência, appsettings.json 
    /// ou código direto no <c>Program.cs</c> ao registrar o provedor de log personalizado.
    /// </para>
    /// </remarks>
    public class CustomLoggerProviderConfiguration
    {
        /// <summary>
        /// Obtém ou define o nível mínimo de log que será registrado.
        /// </summary>
        /// <value>
        /// O valor padrão é <see cref="LogLevel.Information"/>, o que significa que logs 
        /// de nível <c>Information</c> e superior (Warning, Error, Critical) serão capturados.
        /// </value>
        /// <example>
        /// Exemplo de configuração via código:
        /// <code>
        /// var config = new CustomLoggerProviderConfiguration
        /// {
        ///     LogLevel = LogLevel.Warning
        /// };
        /// </code>
        /// </example>
        public LogLevel LogLevel { get; set; } = LogLevel.Information;

        /// <summary>
        /// Obtém ou define o caminho completo do arquivo de log no sistema de arquivos.
        /// </summary>
        /// <value>
        /// O valor padrão é <c>"Logs/sistema-cadastro.log"</c>.
        /// Se o diretório não existir, ele será criado automaticamente.
        /// </value>
        /// <example>
        /// Exemplo de configuração:
        /// <code>
        /// var config = new CustomLoggerProviderConfiguration
        /// {
        ///     LogFilePath = "C:/Logs/Aplicacao.log"
        /// };
        /// </code>
        /// </example>
        public string LogFilePath { get; set; } = "Logs/sistema-cadastro.log";

        /// <summary>
        /// Indica se os logs devem ser gravados em arquivo.
        /// </summary>
        /// <value>
        /// O valor padrão é <c>true</c>.
        /// Quando <c>false</c>, a gravação em arquivo é desativada, mesmo que <see cref="LogFilePath"/> esteja definido.
        /// </value>
        public bool LogToFile { get; set; } = true;

        /// <summary>
        /// Indica se os logs devem ser persistidos no banco de dados.
        /// </summary>
        /// <value>
        /// O valor padrão é <c>true</c>.
        /// Quando <c>false</c>, o <see cref="CustomLogger"/> ignora a gravação de logs na tabela de logs do sistema.
        /// </value>
        public bool LogToDatabase { get; set; } = true;
    }
}
