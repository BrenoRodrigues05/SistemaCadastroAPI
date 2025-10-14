using Microsoft.EntityFrameworkCore;
using SistemaCadastro.Logging;
using SistemaCadastro.Models;

namespace SistemaCadastro.Context
{
    /// <summary>
    /// Contexto principal da aplicação para acesso ao banco de dados.
    /// Configura as entidades do sistema e permite operações de CRUD via Entity Framework Core.
    /// </summary>
    public class SistemaCadastroContext : DbContext
    {
        /// <summary>
        /// Construtor principal utilizado para injeção de dependência com opções do DbContext.
        /// </summary>
        /// <param name="options">Opções de configuração do DbContext, incluindo string de conexão e provedores.</param>
        public SistemaCadastroContext(DbContextOptions<SistemaCadastroContext> options) : base(options)
        {
        }

        /// <summary>
        /// Construtor protegido sem parâmetros, necessário para algumas operações internas do EF Core.
        /// </summary>
        protected SistemaCadastroContext()
        {
        }

        /// <summary>
        /// DbSet representando a tabela de cadastros de usuários.
        /// </summary>
        public DbSet<Cadastro> Cadastros { get; set; }

        /// <summary>
        /// DbSet representando a tabela de logs da API.
        /// Armazena informações de requisições, erros e auditoria.
        /// </summary>
        public DbSet<LogEntry> ApiLogs { get; set; }
    }
}
