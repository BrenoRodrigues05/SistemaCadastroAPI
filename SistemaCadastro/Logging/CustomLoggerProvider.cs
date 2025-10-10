using Microsoft.Extensions.Logging;
using SistemaCadastro.Context;
using System.Collections.Concurrent;

namespace SistemaCadastro.Logging
{
    [ProviderAlias("CustomLogger")]
    public class CustomLoggerProvider : ILoggerProvider
    {
        private readonly CustomLoggerProviderConfiguration _config;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ConcurrentDictionary<string, CustomLogger> _loggers = new();

        public CustomLoggerProvider(
            CustomLoggerProviderConfiguration config,
            IServiceScopeFactory scopeFactory)
        {
            _config = config;
            _scopeFactory = scopeFactory;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name =>
            {
                // Cria um escopo novo pra obter um DbContext
                var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<SistemaCadastroContext>();
                return new CustomLogger(name, _config, context);
            });
        }

        public void Dispose() => _loggers.Clear();
    }
}
