using SistemaCadastro.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SistemaCadastro.Repositories
{
    public interface ICadastroRepository
    {
        Task<IEnumerable<Cadastro>> GetAllAsync();
        Task<Cadastro> GetByIdAsync(int id);
        Task AddAsync(Cadastro cadastro);
        Task UpdateAsync(Cadastro cadastro);
        Task RemoveAsync(int id);

        Task<bool> GetByCpf(string cpf);

        // Método para paginação e filtros
        IQueryable<Cadastro> Query();
    }
}
