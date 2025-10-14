using SistemaCadastro.DTOs;
using SistemaCadastro.Pagination;
using SistemaCadastro.Repositories;

namespace SistemaCadastro.Services;

public class CadastroService
{
    private readonly IUnitOfWork _unitOfWork;

    public CadastroService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<CadastroReadDTO>> GetAllAsync(int pageNumber, int pageSize)
    {

        var query = _unitOfWork.CadastroRepository.Query();

        var paged = await PaginationHelper.GetPagedResultAsync(query, pageNumber, pageSize);

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

        return new PagedResult<CadastroReadDTO>
        {
            Items = dtoList,
            TotalItems = paged.TotalItems,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize
        };

    }

}

