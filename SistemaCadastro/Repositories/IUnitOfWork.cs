using System.Threading.Tasks;

namespace SistemaCadastro.Repositories
{
    /// <summary>
    /// Define o contrato para a Unidade de Trabalho (Unit of Work), coordenando transações e repositórios.
    /// </summary>
    /// <remarks>
    /// A interface <see cref="IUnitOfWork"/> centraliza o gerenciamento de repositórios e garante
    /// que múltiplas operações de banco de dados possam ser commitadas em uma única transação.
    /// </remarks>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Repositório responsável pelas operações de persistência da entidade <see cref="Cadastro"/>.
        /// </summary>
        ICadastroRepository CadastroRepository { get; }

        /// <summary>
        /// Persiste todas as alterações pendentes no banco de dados.
        /// </summary>
        /// <returns>
        /// O número de registros afetados no banco de dados.
        /// </returns>
        /// <remarks>
        /// Deve ser chamado ao final de um conjunto de operações para garantir consistência e integridade.
        /// </remarks>
        Task<int> CommitAsync();
    }
}
