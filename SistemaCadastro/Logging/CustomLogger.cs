using Microsoft.Extensions.Logging;
using SistemaCadastro.Context;
using System;
using System.IO;

namespace SistemaCadastro.Logging
{
    public class CustomLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly CustomLoggerProviderConfiguration _config;
        private readonly SistemaCadastroContext? _context;
        private static readonly object _lock = new();

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

        public IDisposable BeginScope<TState>(TState state) => null!;

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _config.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
                                Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var message = formatter(state, exception);
            var logRecord = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {_categoryName}: {message}";

            if (exception != null)
                logRecord += $"{Environment.NewLine}❌ Exception: {exception.Message}";

            //  Grava em arquivo
            if (_config.LogToFile)
            {
                lock (_lock)
                {
                    File.AppendAllText(_config.LogFilePath, logRecord + Environment.NewLine);
                }
            }

            //  Grava no banco de dados
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

            //  Mostra no console 
            Console.WriteLine(logRecord);
        }
    }
}
