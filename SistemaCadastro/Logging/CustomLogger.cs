using Microsoft.Extensions.Logging;
using SistemaCadastro.Context;
using System;
using System.IO;

namespace SistemaCadastro.Logging
{
    /// <summary>
    /// Implementação personalizada da interface <see cref="ILogger"/> para registrar logs da aplicação.
    /// </summary>
    /// <remarks>
    /// Este logger permite o registro de logs em diferentes destinos simultaneamente:
    /// <list type="bullet">
    /// <item><description> Arquivo de texto (logs persistidos localmente)</description></item>
    /// <item><description> Banco de dados (tabela <c>ApiLogs</c> no contexto <see cref="SistemaCadastroContext"/>)</description></item>
    /// <item><description> Console (para depuração em tempo de execução)</description></item>
    /// </list>
    /// É projetado para ser seguro em ambientes concorrentes e tolerante a falhas — uma exceção durante o log
    /// nunca deve interromper o funcionamento normal da aplicação.
    /// </remarks>
    public class CustomLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly CustomLoggerProviderConfiguration _config;
        private readonly SistemaCadastroContext? _context;
        private static readonly object _lock = new();

        /// <summary>
        /// Inicializa uma nova instância do logger customizado.
        /// </summary>
        /// <param name="categoryName">Nome da categoria (geralmente o nome da classe que gera o log).</param>
        /// <param name="config">Configurações do logger, definindo níveis de log e destinos de saída.</param>
        /// <param name="context">Contexto do banco de dados opcional, utilizado quando <see cref="CustomLoggerProviderConfiguration.LogToDatabase"/> é verdadeiro.</param>
        /// <remarks>
        /// Caso a opção <see cref="CustomLoggerProviderConfiguration.LogToFile"/> esteja habilitada,
        /// o diretório de destino será criado automaticamente se não existir.
        /// </remarks>
        public CustomLogger(string categoryName, CustomLoggerProviderConfiguration config, SistemaCadastroContext? context = null)
        {
            _categoryName = categoryName;
            _config = config;
            _context = context;

            if (_config.LogToFile)
            {
                var dir = Path.GetDirectoryName(_config.LogFilePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// Inicia um escopo de log (não utilizado nesta implementação).
        /// </summary>
        /// <typeparam name="TState">Tipo do estado do escopo.</typeparam>
        /// <param name="state">Estado contextual do log.</param>
        /// <returns>Sempre retorna <c>null</c>, pois o escopo não é implementado.</returns>
        public IDisposable BeginScope<TState>(TState state) => null!;

        /// <summary>
        /// Verifica se o nível de log está habilitado de acordo com a configuração.
        /// </summary>
        /// <param name="logLevel">Nível de log a ser verificado.</param>
        /// <returns><c>true</c> se o nível for igual ou superior ao configurado; caso contrário, <c>false</c>.</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _config.LogLevel;
        }

        /// <summary>
        /// Registra um log de acordo com o nível, mensagem e exceção informados.
        /// </summary>
        /// <typeparam name="TState">Tipo do estado do log, geralmente um objeto ou string.</typeparam>
        /// <param name="logLevel">Nível de severidade do log (Information, Warning, Error, etc.).</param>
        /// <param name="eventId">Identificador opcional do evento.</param>
        /// <param name="state">Objeto que contém a mensagem ou dados do log.</param>
        /// <param name="exception">Exceção opcional associada ao log.</param>
        /// <param name="formatter">Função responsável por formatar a mensagem de log.</param>
        /// <remarks>
        /// O log é gravado de acordo com as opções definidas na configuração:
        /// <list type="bullet">
        /// <item><description><strong>Arquivo:</strong> salvo de forma thread-safe usando bloqueio.</description></item>
        /// <item><description><strong>Banco de dados:</strong> armazenado na tabela <c>ApiLogs</c> via <see cref="SistemaCadastroContext"/>.</description></item>
        /// <item><description><strong>Console:</strong> impresso no terminal de execução.</description></item>
        /// </list>
        /// Exceções internas durante a gravação do log são capturadas silenciosamente para não afetar o fluxo da aplicação.
        /// </remarks>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
                                Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var message = formatter(state, exception);
            var logRecord = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {_categoryName}: {message}";

            if (exception != null)
                logRecord += $"{Environment.NewLine}❌ Exception: {exception.Message}";

            // Grava em arquivo
            if (_config.LogToFile)
            {
                lock (_lock)
                {
                    File.AppendAllText(_config.LogFilePath, logRecord + Environment.NewLine);
                }
            }

            // Grava no banco de dados
            if (_config.LogToDatabase && _context != null)
            {
                try
                {
                    var log = new LogEntry
                    {
                        Timestamp = DateTime.UtcNow,
                        LogLevel = logLevel.ToString(),
                        Category = _categoryName,
                        Message = message,
                        Exception = exception?.ToString()
                    };

                    _context.ApiLogs.Add(log);
                    _context.SaveChanges();
                }
                catch
                {
                    // Falha no log nunca deve derrubar a aplicação
                }
            }

            // Exibe no console
            Console.WriteLine(logRecord);
        }
    }
}
