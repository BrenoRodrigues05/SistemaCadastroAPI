namespace SistemaCadastro.Interfaces
{
    public interface IDTOMapper
    {
        // Interface genérica para mapeamento manual entre entidade e DTO
        public interface IDTOMapper<TEntity, TReadDTO, TCreateDTO>
            where TEntity : class
            where TReadDTO : class
            where TCreateDTO : class
        {
            TReadDTO ToReadDTO(TEntity entity);
            TEntity ToEntity(TCreateDTO dto);
            void MapToEntity(TCreateDTO dto, TEntity entity);
        }
    }
}
