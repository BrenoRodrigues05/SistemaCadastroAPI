using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SistemaCadastro.Context
{
    /// <summary>
    /// Fábrica de contexto para design-time do Entity Framework Core.
    /// Utilizada para migrações e ferramentas do EF Core que precisam instanciar o DbContext fora da execução da aplicação.
    /// </summary>
    public class SistemaCadastroContextFactory : IDesignTimeDbContextFactory<SistemaCadastroContext>
    {
        /// <summary>
        /// Cria uma instância do <see cref="SistemaCadastroContext"/> para uso em design-time.
        /// </summary>
        /// <param name="args">Argumentos de linha de comando (não utilizados neste método).</param>
        /// <returns>Uma nova instância configurada do <see cref="SistemaCadastroContext"/>.</returns>
        public SistemaCadastroContext CreateDbContext(string[] args)
        {
            // String de conexão para o banco de dados MySQL
            var connectionString = "server=localhost;database=CadastrosDB;uid=root;pwd=05121999";

            var optionsBuilder = new DbContextOptionsBuilder<SistemaCadastroContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new SistemaCadastroContext(optionsBuilder.Options);
        }
    }
}
