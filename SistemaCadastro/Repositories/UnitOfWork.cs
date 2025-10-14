using SistemaCadastro.Context;
using SistemaCadastro.Models;
using System;
using System.Threading.Tasks;

namespace SistemaCadastro.Repositories
{
    /// <summary>
    /// Implementação do padrão Unit of Work, responsável por coordenar os repositórios
    /// e gerenciar transações no contexto <see cref="SistemaCadastroContext"/>.
    /// </summary>
    /// <remarks>
    /// Esta classe garante que múltiplas operações sobre repositórios diferentes sejam
    /// commitadas de forma atômica e que o contexto do EF Core seja corretamente descartado.
    /// </remarks>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SistemaCadastroContext _context;

        /// <summary>
        /// Repositório responsável pelas operações de persistência da entidade <see cref="Cadastro"/>.
        /// </summary>
        public ICadastroRepository CadastroRepository { get; }

        /// <summary>
        /// Inicializa uma nova instância do <see cref="UnitOfWork"/> com o contexto fornecido.
        /// </summary>
        /// <param name="context">Instância do <see cref="SistemaCadastroContext"/> para acesso ao banco de dados.</param>
        public UnitOfWork(SistemaCadastroContext context)
        {
            _context = context;
            CadastroRepository = new CadastroRepository(_context);
        }

        /// <summary>
        /// Persiste todas as alterações pendentes no banco de dados de forma assíncrona.
        /// </summary>
        /// <returns>O número de registros afetados no banco de dados.</returns>
        /// <remarks>
        /// Deve ser chamado ao final de um conjunto de operações sobre os repositórios
        /// para garantir consistência e integridade das transações.
        /// </remarks>
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Libera os recursos utilizados pelo contexto do Entity Framework.
        /// </summary>
        /// <remarks>
        /// Chamado automaticamente ao final do ciclo de vida do Unit of Work ou
        /// via instrução <c>using</c> quando implementado em escopo.
        /// </remarks>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
