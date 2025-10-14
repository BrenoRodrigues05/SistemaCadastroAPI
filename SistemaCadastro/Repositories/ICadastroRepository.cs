using SistemaCadastro.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaCadastro.Repositories
{
    /// <summary>
    /// Define o contrato para operações de persistência e consultas sobre a entidade <see cref="Cadastro"/>.
    /// </summary>
    /// <remarks>
    /// Este repositório encapsula as operações de CRUD, verificações de CPF e fornece suporte a consultas
    /// paginadas ou filtradas via <see cref="IQueryable{Cadastro}"/>.
    /// </remarks>
    public interface ICadastroRepository
    {
        /// <summary>
        /// Recupera todos os cadastros existentes.
        /// </summary>
        /// <returns>Uma lista de objetos <see cref="Cadastro"/>.</returns>
        Task<IEnumerable<Cadastro>> GetAllAsync();

        /// <summary>
        /// Recupera um cadastro pelo seu identificador.
        /// </summary>
        /// <param name="id">ID do cadastro a ser buscado.</param>
        /// <returns>
        /// O cadastro correspondente ou <c>null</c> caso não seja encontrado.
        /// </returns>
        Task<Cadastro> GetByIdAsync(int id);

        /// <summary>
        /// Adiciona um novo cadastro ao banco de dados.
        /// </summary>
        /// <param name="cadastro">Instância de <see cref="Cadastro"/> a ser adicionada.</param>
        Task AddAsync(Cadastro cadastro);

        /// <summary>
        /// Atualiza um cadastro existente no banco de dados.
        /// </summary>
        /// <param name="cadastro">Instância de <see cref="Cadastro"/> com os dados atualizados.</param>
        Task UpdateAsync(Cadastro cadastro);

        /// <summary>
        /// Remove um cadastro pelo seu identificador.
        /// </summary>
        /// <param name="id">ID do cadastro a ser removido.</param>
        Task RemoveAsync(int id);

        /// <summary>
        /// Verifica se já existe um cadastro com o CPF informado.
        /// </summary>
        /// <param name="cpf">CPF a ser verificado.</param>
        /// <returns>
        /// <c>true</c> se existir um cadastro com o CPF informado, <c>false</c> caso contrário.
        /// </returns>
        Task<bool> GetByCpf(string cpf);

        /// <summary>
        /// Retorna uma consulta <see cref="IQueryable{Cadastro}"/> para permitir paginação, filtros ou ordenação.
        /// </summary>
        /// <remarks>
        /// Este método é útil para construir consultas personalizadas sem executar imediatamente o acesso ao banco de dados.
        /// </remarks>
        /// <returns>Uma consulta <see cref="IQueryable{Cadastro}"/> representando todos os cadastros.</returns>
        IQueryable<Cadastro> Query();
    }
}
