using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaCadastro.Logging;
using SistemaCadastro.Models;

namespace SistemaCadastro.Context
{
    /// <summary>
    /// Contexto principal da aplicação para acesso ao banco de dados.
    /// Configura as entidades do sistema e permite operações de CRUD via Entity Framework Core.
    /// </summary>
    public class SistemaCadastroContext : IdentityDbContext <ApplicationUser>
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

        /// <summary>
        /// Configura o modelo de dados e mapeamentos das entidades para o banco de dados.
        /// </summary>
        /// <param name="builder">Instância do <see cref="ModelBuilder"/> usada para configurar as entidades.</param>
        /// <remarks>
        /// Este método é chamado pelo Entity Framework Core durante a criação do contexto.
        /// Pode ser usado para:
        /// <list type="bullet">
        /// <item>Configurar chaves primárias e estrangeiras.</item>
        /// <item>Definir relacionamentos entre entidades.</item>
        /// <item>Aplicar constraints e índices.</item>
        /// <item>Mapear nomes de tabelas e colunas.</item>
        /// </list>
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
