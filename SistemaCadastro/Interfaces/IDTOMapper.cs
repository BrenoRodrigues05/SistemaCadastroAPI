namespace SistemaCadastro.Interfaces
{
    /// <summary>
    /// Define contratos para mapeamento manual entre entidades e seus respectivos DTOs.
    /// </summary>
    /// <remarks>
    /// Esta interface é usada para garantir uma padronização na conversão de dados entre
    /// entidades de domínio e objetos de transferência de dados (DTOs).
    /// <para>
    /// O objetivo é centralizar a lógica de conversão, evitando duplicação de código
    /// e facilitando manutenção em operações de criação, leitura e atualização.
    /// </para>
    /// </remarks>
    public interface IDTOMapper
    {
        /// <summary>
        /// Interface genérica para mapeamento entre uma entidade e seus DTOs de leitura e criação.
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade de domínio.</typeparam>
        /// <typeparam name="TReadDTO">Tipo do DTO usado para leitura de dados.</typeparam>
        /// <typeparam name="TCreateDTO">Tipo do DTO usado para criação ou atualização de dados.</typeparam>
        /// <remarks>
        /// Essa interface deve ser implementada para cada par de entidade/DTOs, 
        /// permitindo a conversão manual entre modelos de domínio e representações externas.
        /// </remarks>
        public interface IDTOMapper<TEntity, TReadDTO, TCreateDTO>
            where TEntity : class
            where TReadDTO : class
            where TCreateDTO : class
        {
            /// <summary>
            /// Converte uma entidade de domínio em um DTO de leitura.
            /// </summary>
            /// <param name="entity">Entidade a ser convertida.</param>
            /// <returns>Um objeto do tipo <typeparamref name="TReadDTO"/> contendo os dados da entidade.</returns>
            /// <example>
            /// Exemplo de uso:
            /// <code>
            /// var dto = mapper.ToReadDTO(usuario);
            /// </code>
            /// </example>
            TReadDTO ToReadDTO(TEntity entity);

            /// <summary>
            /// Converte um DTO de criação em uma nova instância da entidade correspondente.
            /// </summary>
            /// <param name="dto">Objeto DTO contendo os dados de entrada.</param>
            /// <returns>Uma nova instância de <typeparamref name="TEntity"/> preenchida com os dados do DTO.</returns>
            /// <example>
            /// <code>
            /// var entity = mapper.ToEntity(dto);
            /// </code>
            /// </example>
            TEntity ToEntity(TCreateDTO dto);

            /// <summary>
            /// Atualiza uma entidade existente com base nos dados de um DTO.
            /// </summary>
            /// <param name="dto">Objeto DTO contendo os novos valores.</param>
            /// <param name="entity">Instância da entidade a ser atualizada.</param>
            /// <remarks>
            /// Este método é útil em operações <c>PATCH</c> ou <c>PUT</c>,
            /// onde se deseja atualizar apenas determinados campos de uma entidade existente.
            /// </remarks>
            /// <example>
            /// <code>
            /// mapper.MapToEntity(dto, usuarioExistente);
            /// </code>
            /// </example>
            void MapToEntity(TCreateDTO dto, TEntity entity);
        }
    }
}
