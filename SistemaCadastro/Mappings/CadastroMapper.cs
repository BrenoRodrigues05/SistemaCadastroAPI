using SistemaCadastro.DTOs;
using SistemaCadastro.Models;
using static SistemaCadastro.Interfaces.IDTOMapper;

namespace SistemaCadastro.Mappings
{
    /// <summary>
    /// Responsável pelo mapeamento manual entre a entidade <see cref="Cadastro"/> e seus respectivos DTOs.
    /// </summary>
    /// <remarks>
    /// Esta classe implementa a interface genérica <see cref="IDTOMapper{TEntity, TReadDTO, TCreateDTO}"/>,
    /// definindo as regras de conversão entre as representações de dados utilizadas na API.
    /// <para>
    /// O mapeamento manual garante controle total sobre as transformações e facilita a aplicação de
    /// regras de negócio ou formatações específicas entre as camadas de domínio e apresentação.
    /// </para>
    /// </remarks>
    public class CadastroMapper : IDTOMapper<Cadastro, CadastroReadDTO, CadastroCreateDTO>
    {
        /// <summary>
        /// Converte uma entidade <see cref="Cadastro"/> em um objeto <see cref="CadastroReadDTO"/> 
        /// para leitura e retorno em respostas da API.
        /// </summary>
        /// <param name="entity">Instância da entidade <see cref="Cadastro"/> a ser convertida.</param>
        /// <returns>
        /// Um objeto <see cref="CadastroReadDTO"/> contendo os dados da entidade prontos para exibição.
        /// </returns>
        /// <example>
        /// <code>
        /// var mapper = new CadastroMapper();
        /// var dto = mapper.ToReadDTO(cadastro);
        /// Console.WriteLine(dto.Nome);
        /// </code>
        /// </example>
        public CadastroReadDTO ToReadDTO(Cadastro entity)
        {
            return new CadastroReadDTO
            {
                Id = entity.Id,
                Cpf = entity.Cpf,
                Nome = entity.Nome,
                Email = entity.Email,
                Telefone = entity.Telefone,
                Nascimento = entity.Nascimento,
                Estado = entity.Estado,
                Cidade = entity.Cidade,
                Cargo = entity.Cargo
            };
        }

        /// <summary>
        /// Converte um objeto <see cref="CadastroCreateDTO"/> em uma nova instância da entidade <see cref="Cadastro"/>.
        /// </summary>
        /// <param name="dto">DTO de criação contendo os dados de entrada fornecidos pelo usuário.</param>
        /// <returns>
        /// Uma nova entidade <see cref="Cadastro"/> pronta para ser persistida no banco de dados.
        /// </returns>
        /// <example>
        /// <code>
        /// var mapper = new CadastroMapper();
        /// var entidade = mapper.ToEntity(dto);
        /// context.Cadastros.Add(entidade);
        /// </code>
        /// </example>
        public Cadastro ToEntity(CadastroCreateDTO dto)
        {
            return new Cadastro
            {
                Cpf = dto.Cpf,
                Nome = dto.Nome,
                Email = dto.Email,
                Telefone = dto.Telefone,
                Nascimento = dto.Nascimento,
                Estado = dto.Estado,
                Cidade = dto.Cidade,
                Cargo = dto.Cargo
            };
        }

        /// <summary>
        /// Atualiza os dados de uma entidade <see cref="Cadastro"/> existente com base nos valores de um DTO.
        /// </summary>
        /// <param name="dto">DTO contendo os novos dados a serem aplicados.</param>
        /// <param name="entity">Instância da entidade <see cref="Cadastro"/> que será atualizada.</param>
        /// <remarks>
        /// Este método é útil em operações de atualização (PUT ou PATCH),
        /// evitando a necessidade de recriar o objeto e mantendo o rastreamento do EF Core.
        /// </remarks>
        /// <example>
        /// <code>
        /// var mapper = new CadastroMapper();
        /// mapper.MapToEntity(dto, cadastroExistente);
        /// context.Update(cadastroExistente);
        /// </code>
        /// </example>
        public void MapToEntity(CadastroCreateDTO dto, Cadastro entity)
        {
            entity.Cpf = dto.Cpf;
            entity.Nome = dto.Nome;
            entity.Email = dto.Email;
            entity.Telefone = dto.Telefone;
            entity.Nascimento = dto.Nascimento;
            entity.Estado = dto.Estado;
            entity.Cidade = dto.Cidade;
            entity.Cargo = dto.Cargo;
        }
    }
}
