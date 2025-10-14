using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaCadastro.Pagination
{
    /// <summary>
    /// Fornece métodos auxiliares para aplicar paginação em consultas <see cref="IQueryable{T}"/>.
    /// </summary>
    /// <remarks>
    /// Esta classe permite criar resultados paginados de forma assíncrona a partir de consultas EF Core,
    /// retornando um objeto <see cref="PagedResult{T}"/> com metadados úteis como número total de registros,
    /// página atual e tamanho da página.
    /// </remarks>
    public static class PaginationHelper
    {
        /// <summary>
        /// Executa uma consulta paginada de forma assíncrona, retornando os itens da página solicitada e os metadados de paginação.
        /// </summary>
        /// <typeparam name="T">Tipo de entidade ou DTO a ser retornado na consulta.</typeparam>
        /// <param name="query">Consulta base do Entity Framework Core que será paginada.</param>
        /// <param name="pageNumber">Número da página atual (baseado em 1).</param>
        /// <param name="pageSize">Quantidade de registros por página.</param>
        /// <returns>
        /// Um objeto <see cref="PagedResult{T}"/> contendo os itens da página atual e informações de paginação.
        /// </returns>
        /// <example>
        /// Exemplo de uso:
        /// <code>
        /// var resultado = await PaginationHelper.GetPagedResultAsync(context.Funcionarios, 1, 10);
        /// Console.WriteLine($"Página {resultado.PageNumber} de {resultado.TotalPages}");
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">Lançada se a consulta (<paramref name="query"/>) for nula.</exception>
        public static async Task<PagedResult<T>> GetPagedResultAsync<T>(
            IQueryable<T> query,
            int pageNumber,
            int pageSize)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query), "A consulta fornecida não pode ser nula.");

            // Garante que o número da página e o tamanho sejam válidos
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            // Conta o total de registros antes da aplicação da paginação
            var totalItems = await query.CountAsync();

            // Aplica os limites de página
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
