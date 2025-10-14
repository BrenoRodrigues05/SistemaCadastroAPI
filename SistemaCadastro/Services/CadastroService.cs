using SistemaCadastro.DTOs;
using SistemaCadastro.Pagination;
using SistemaCadastro.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaCadastro.Services
{
    /// <summary>
    /// Serviço responsável pelas operações de negócios relacionadas à entidade <see cref="SistemaCadastro.Models.Cadastro"/>.
    /// </summary>
    /// <remarks>
    /// Encapsula a lógica de consulta, mapeamento para DTOs e paginação, desacoplando os controladores do acesso direto ao repositório.
    /// </remarks>
    public class CadastroService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="CadastroService"/>.
        /// </summary>
        /// <param name="unitOfWork">Instância do <see cref="IUnitOfWork"/> para acessar os repositórios.</param>
        public CadastroService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Recupera uma lista paginada de cadastros, convertendo-os para <see cref="CadastroReadDTO"/>.
        /// </summary>
        /// <param name="pageNumber">Número da página a ser retornada (base 1).</param>
        /// <param name="pageSize">Quantidade de registros por página.</param>
        /// <returns>
        /// Um objeto <see cref="PagedResult{T}"/> contendo os cadastros convertidos para DTOs
        /// e os metadados de paginação (total de itens, página atual, tamanho da página).
        /// </returns>
        public async Task<PagedResult<CadastroReadDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            // Consulta base para paginação
            var query = _unitOfWork.CadastroRepository.Query();

            // Aplica paginação via helper
            var paged = await PaginationHelper.GetPagedResultAsync(query, pageNumber, pageSize);

            // Mapeia cada entidade para DTO
            var dtoList = paged.Items.Select(c => new CadastroReadDTO
            {
                Id = c.Id,
                Cpf = c.Cpf,
                Nome = c.Nome,
                Email = c.Email,
                Telefone = c.Telefone,
                Nascimento = c.Nascimento,
                Estado = c.Estado,
                Cidade = c.Cidade,
                Cargo = c.Cargo
            });

            // Retorna resultado paginado com DTOs
            return new PagedResult<CadastroReadDTO>
            {
                Items = dtoList,
                TotalItems = paged.TotalItems,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize
            };
        }
    }
}
