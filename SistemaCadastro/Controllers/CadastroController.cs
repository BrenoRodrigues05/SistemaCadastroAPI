using Microsoft.AspNetCore.Mvc;
using SistemaCadastro.DTOs;
using SistemaCadastro.Mappings;
using SistemaCadastro.Models;
using SistemaCadastro.Pagination;
using SistemaCadastro.Repositories;

namespace SistemaCadastro.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de cadastros de usuários.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CadastroController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CadastroController> _logger;
        private readonly CadastroMapper _mapper;

        public CadastroController(IUnitOfWork unitOfWork, ILogger<CadastroController> logger, CadastroMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Retorna todos os cadastros.
        /// </summary>
        /// <returns>Lista de cadastros.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        /// <example>
        /// [
        ///   {
        ///     "id": 1,
        ///     "cpf": "123.456.789-00",
        ///     "nome": "João Silva",
        ///     "email": "joao@example.com",
        ///     "telefone": "(11) 99999-9999",
        ///     "nascimento": "1990-01-01",
        ///     "estado": "São Paulo",
        ///     "cidade": "São Paulo",
        ///     "cargo": "Desenvolvedor"
        ///   }
        /// ]
        /// </example>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CadastroReadDTO>>> GetAll()
        {
            var cadastros = await _unitOfWork.CadastroRepository.GetAllAsync();
            var dtos = cadastros.Select(c => _mapper.ToReadDTO(c));
            return Ok(dtos);
        }

        /// <summary>
        /// Retorna um cadastro pelo ID.
        /// </summary>
        /// <param name="id">ID do cadastro.</param>
        /// <returns>Cadastro correspondente ao ID.</returns>
        /// <response code="200">Cadastro encontrado.</response>
        /// <response code="404">Cadastro não encontrado.</response>
        /// <example>
        /// {
        ///   "id": 1,
        ///   "cpf": "123.456.789-00",
        ///   "nome": "João Silva",
        ///   "email": "joao@example.com",
        ///   "telefone": "(11) 99999-9999",
        ///   "nascimento": "1990-01-01",
        ///   "estado": "São Paulo",
        ///   "cidade": "São Paulo",
        ///   "cargo": "Desenvolvedor"
        /// }
        /// </example>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CadastroReadDTO>> GetById(int id)
        {
            var cadastro = await _unitOfWork.CadastroRepository.GetByIdAsync(id);
            if (cadastro == null)
                return NotFound(new { Message = $"Cadastro com ID {id} não encontrado." });

            return Ok(_mapper.ToReadDTO(cadastro));
        }

        /// <summary>
        /// Retorna um cadastro pelo CPF.
        /// </summary>
        /// <param name="cpf">CPF do usuário.</param>
        /// <returns>Cadastro correspondente ao CPF.</returns>
        /// <response code="200">Cadastro encontrado.</response>
        /// <response code="404">Cadastro não encontrado.</response>
        [HttpGet("cpf/{cpf}")]
        public async Task<ActionResult<CadastroReadDTO>> GetByCpf(string cpf)
        {
            var cadastros = await _unitOfWork.CadastroRepository.GetAllAsync();
            var cadastro = cadastros.FirstOrDefault(c => c.Cpf == cpf);

            if (cadastro == null)
                return NotFound(new { Message = $"Cadastro com CPF {cpf} não encontrado." });

            return Ok(_mapper.ToReadDTO(cadastro));
        }

        /// <summary>
        /// Cria um novo cadastro.
        /// </summary>
        /// <param name="dto">Dados do cadastro.</param>
        /// <response code="201">Cadastro criado com sucesso.</response>
        /// <response code="400">CPF já existe ou dados inválidos.</response>
        /// <example>
        /// {
        ///   "cpf": "123.456.789-00",
        ///   "nome": "João Silva",
        ///   "email": "joao@example.com",
        ///   "telefone": "(11) 99999-9999",
        ///   "nascimento": "1990-01-01",
        ///   "estado": "São Paulo",
        ///   "cidade": "São Paulo",
        ///   "cargo": "Desenvolvedor"
        /// }
        /// </example>
        [HttpPost]
        public async Task<ActionResult<CadastroReadDTO>> Create([FromBody] CadastroCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _unitOfWork.CadastroRepository.GetByCpf(dto.Cpf!))
                return BadRequest(new { Message = "CPF já cadastrado no sistema." });

            var cadastro = _mapper.ToEntity(dto);
            await _unitOfWork.CadastroRepository.AddAsync(cadastro);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetById), new { id = cadastro.Id }, _mapper.ToReadDTO(cadastro));
        }

        /// <summary>
        /// Atualiza completamente um cadastro existente.
        /// </summary>
        /// <param name="id">ID do cadastro.</param>
        /// <param name="dto">Novos dados do cadastro.</param>
        /// <response code="204">Atualização realizada com sucesso.</response>
        /// <response code="404">Cadastro não encontrado.</response>
        /// <example>
        /// {
        ///   "cpf": "123.456.789-00",
        ///   "nome": "João Silva",
        ///   "email": "joao@novoemail.com",
        ///   "telefone": "(11) 99999-9999",
        ///   "nascimento": "1990-01-01",
        ///   "estado": "São Paulo",
        ///   "cidade": "São Paulo",
        ///   "cargo": "Desenvolvedor Sênior"
        /// }
        /// </example>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] CadastroCreateDTO dto)
        {
            var existing = await _unitOfWork.CadastroRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = $"Cadastro com ID {id} não encontrado." });

            _mapper.MapToEntity(dto, existing);
            await _unitOfWork.CadastroRepository.UpdateAsync(existing);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }

        /// <summary>
        /// Atualiza parcialmente um cadastro existente.
        /// </summary>
        /// <param name="id">ID do cadastro.</param>
        /// <param name="dto">Campos a atualizar.</param>
        /// <response code="200">Cadastro atualizado parcialmente.</response>
        /// <response code="404">Cadastro não encontrado.</response>
        /// <example>
        /// {
        ///   "nome": "João Atualizado",
        ///   "email": "joaoatualizado@example.com"
        /// }
        /// </example>
        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, [FromBody] CadastroPatchDTO dto)
        {
            var cadastro = await _unitOfWork.CadastroRepository.GetByIdAsync(id);
            if (cadastro == null)
                return NotFound(new { Message = $"Cadastro com ID {id} não encontrado." });

            if (!string.IsNullOrWhiteSpace(dto.Cpf)) cadastro.Cpf = dto.Cpf;
            if (!string.IsNullOrWhiteSpace(dto.Nome)) cadastro.Nome = dto.Nome;
            if (!string.IsNullOrWhiteSpace(dto.Email)) cadastro.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.Telefone)) cadastro.Telefone = dto.Telefone;
            if (dto.Nascimento != default) cadastro.Nascimento = dto.Nascimento;
            if (!string.IsNullOrWhiteSpace(dto.Estado)) cadastro.Estado = dto.Estado;
            if (!string.IsNullOrWhiteSpace(dto.Cidade)) cadastro.Cidade = dto.Cidade;
            if (!string.IsNullOrWhiteSpace(dto.Cargo)) cadastro.Cargo = dto.Cargo;

            await _unitOfWork.CadastroRepository.UpdateAsync(cadastro);
            await _unitOfWork.CommitAsync();

            return Ok(_mapper.ToReadDTO(cadastro));
        }

        /// <summary>
        /// Retorna cadastros paginados.
        /// </summary>
        /// <param name="pageNumber">Número da página (padrão: 1).</param>
        /// <param name="pageSize">Tamanho da página (padrão: 10).</param>
        /// <response code="200">Lista de cadastros paginada.</response>
        /// <response code="400">Parâmetros inválidos.</response>
        /// <example>
        /// {
        ///   "items": [
        ///     {
        ///       "id": 1,
        ///       "cpf": "123.456.789-00",
        ///       "nome": "João Silva",
        ///       "email": "joao@example.com",
        ///       "telefone": "(11) 99999-9999",
        ///       "nascimento": "1990-01-01",
        ///       "estado": "São Paulo",
        ///       "cidade": "São Paulo",
        ///       "cargo": "Desenvolvedor"
        ///     }
        ///   ],
        ///   "totalItems": 50,
        ///   "pageNumber": 1,
        ///   "pageSize": 10
        /// }
        /// </example>
        [HttpGet("paginado")]
        public async Task<ActionResult<PagedResult<CadastroReadDTO>>> GetPaged(
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest(new { Message = "pageNumber e pageSize devem ser maiores que zero." });

            var query = _unitOfWork.CadastroRepository.Query();
            var paged = await PaginationHelper.GetPagedResultAsync(query, pageNumber, pageSize);
            var dtoList = paged.Items.Select(c => _mapper.ToReadDTO(c));

            return Ok(new PagedResult<CadastroReadDTO>
            {
                Items = dtoList,
                TotalItems = paged.TotalItems,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize
            });
        }
    }
}
