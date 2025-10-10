using Microsoft.EntityFrameworkCore;
using SistemaCadastro.Logging;
using SistemaCadastro.Models;

namespace SistemaCadastro.Context
{
    public class SistemaCadastroContext : DbContext
    {
        public SistemaCadastroContext(DbContextOptions<SistemaCadastroContext> options) : base(options)
        {
        }
        protected SistemaCadastroContext()
        {
        }

        public DbSet<Cadastro> Cadastros { get; set; }
        public DbSet<LogEntry> ApiLogs { get; set; }
    }
}
