using SistemaCadastro.DTOs;
using SistemaCadastro.Models;
using static SistemaCadastro.Interfaces.IDTOMapper;

namespace SistemaCadastro.Mappings
{
    public class CadastroMapper : IDTOMapper<Cadastro, CadastroReadDTO, CadastroCreateDTO>
    {
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
