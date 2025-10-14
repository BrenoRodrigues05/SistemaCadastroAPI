using System;

namespace SistemaCadastro.Pagination
{
    /// <summary>
    /// Representa o resultado paginado de uma consulta genérica.
    /// </summary>
    /// <typeparam name="T">
    /// Tipo de entidade, DTO ou modelo retornado na lista de resultados.
    /// </typeparam>
    /// <remarks>
    /// A classe <see cref="PagedResult{T}"/> é usada para encapsular o resultado
    /// de consultas com paginação em APIs e repositórios genéricos.
    /// <para>
    /// Inclui metadados úteis para controle de navegação, como o número total de itens,
    /// a página atual e a quantidade de páginas calculada automaticamente.
    /// </para>
    /// </remarks>
    public class PagedResult<T>
    {
        /// <summary>
        /// Lista de itens da página atual.
        /// </summary>
        /// <remarks>
        /// Contém apenas os registros referentes à página solicitada, de acordo com os parâmetros de paginação.
        /// </remarks>
        public IEnumerable<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Número total de registros disponíveis na consulta sem paginação.
        /// </summary>
        /// <example>Por exemplo, se houver 120 cadastros e <see cref="PageSize"/> for 10, este valor será 120.</example>
        public int TotalItems { get; set; }

        /// <summary>
        /// Número da página atual (baseado em 1).
        /// </summary>
        /// <remarks>
        /// Valores menores que 1 devem ser tratados como 1 pelo serviço de paginação.
        /// </remarks>
        public int PageNumber { get; set; }

        /// <summary>
        /// Quantidade de registros exibidos por página.
        /// </summary>
        /// <remarks>
        /// Define o tamanho da coleção retornada em <see cref="Items"/>.
        /// </remarks>
        public int PageSize { get; set; }

        /// <summary>
        /// Quantidade total de páginas calculada com base em <see cref="TotalItems"/> e <see cref="PageSize"/>.
        /// </summary>
        /// <remarks>
        /// O valor é arredondado para cima com <see cref="Math.Ceiling(double)"/>, garantindo que
        /// registros remanescentes formem uma última página parcial, se necessário.
        /// </remarks>
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
