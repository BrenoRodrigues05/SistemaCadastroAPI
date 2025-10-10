using SistemaCadastro.Models;

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
    }
}
