using Microsoft.Extensions.Logging;
using SistemaCadastro.Context;
using System.Collections.Concurrent;

namespace SistemaCadastro.Logging
{
    /// <summary>
    /// Provedor personalizado de log para o sistema, responsável por criar e gerenciar instâncias de <see cref="CustomLogger"/>.
    /// </summary>
    /// <remarks>
    /// Este provedor implementa a interface <see cref="ILoggerProvider"/>, permitindo a integração do <see cref="CustomLogger"/>
    /// com o sistema de logging padrão do ASP.NET Core.
    /// <para>
    /// O provedor é capaz de criar loggers por categoria (geralmente o nome da classe ou namespace) e reutilizá-los
    /// de forma thread-safe através de um <see cref="ConcurrentDictionary{TKey, TValue}"/>.
    /// </para>
    /// <para>
    /// Cada instância de logger recebe um escopo de serviço para resolver o <see cref="SistemaCadastroContext"/>,
    /// garantindo o isolamento de contexto e evitando problemas de concorrência no Entity Framework.
    /// </para>
    /// </remarks>
    [ProviderAlias("CustomLogger")]
    public class CustomLoggerProvider : ILoggerProvider
    {
        private readonly CustomLoggerProviderConfiguration _config;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ConcurrentDictionary<string, CustomLogger> _loggers = new();

        /// <summary>
        /// Inicializa uma nova instância do <see cref="CustomLoggerProvider"/>.
        /// </summary>
        /// <param name="config">Configurações do provedor, incluindo nível de log e opções de destino (arquivo, banco, console).</param>
        /// <param name="scopeFactory">
        /// Fábrica de escopos de serviço usada para criar instâncias isoladas de <see cref="SistemaCadastroContext"/> 
        /// para cada logger criado.
        /// </param>
        public CustomLoggerProvider(
            CustomLoggerProviderConfiguration config,
            IServiceScopeFactory scopeFactory)
        {
            _config = config;
            _scopeFactory = scopeFactory;
        }

        /// <summary>
        /// Cria ou recupera uma instância existente de <see cref="CustomLogger"/> associada à categoria especificada.
        /// </summary>
        /// <param name="categoryName">Nome da categoria do logger (geralmente o nome da classe chamadora).</param>
        /// <returns>Uma instância configurada de <see cref="CustomLogger"/>.</returns>
        /// <remarks>
        /// Cada categoria possui sua própria instância de logger, reutilizada de forma segura por meio de um dicionário concorrente.
        /// <para>
        /// Para cada logger criado, é gerado um novo escopo de serviço a fim de obter uma instância independente
        /// do <see cref="SistemaCadastroContext"/>, garantindo que as operações de log em banco não interfiram entre si.
        /// </para>
        /// </remarks>
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name =>
            {
                // Cria um novo escopo para obter um DbContext isolado
                var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<SistemaCadastroContext>();
                return new CustomLogger(name, _config, context);
            });
        }

        /// <summary>
        /// Libera os recursos utilizados pelo provedor de logger.
        /// </summary>
        /// <remarks>
        /// A limpeza consiste apenas em esvaziar o cache de loggers, uma vez que cada logger gerencia seu próprio ciclo de vida.
        /// </remarks>
        public void Dispose() => _loggers.Clear();
    }
}
