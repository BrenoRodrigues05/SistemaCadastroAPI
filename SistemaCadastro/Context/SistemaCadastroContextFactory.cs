using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SistemaCadastro.Context
{
    public class SistemaCadastroContextFactory : IDesignTimeDbContextFactory<SistemaCadastroContext>
    {
        public SistemaCadastroContext CreateDbContext(string[] args)
        {
            var connectionString = "server=localhost;database=CadastrosDB;uid=root;pwd=05121999";
            var optionsBuilder = new DbContextOptionsBuilder<SistemaCadastroContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new SistemaCadastroContext(optionsBuilder.Options);
        }
    }
}
