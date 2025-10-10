using Microsoft.AspNetCore.Mvc;
using SistemaCadastro.DTOs;
using SistemaCadastro.Mappings;
using SistemaCadastro.Models;
using SistemaCadastro.Repositories;

namespace SistemaCadastro.Controllers
{
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CadastroReadDTO>>> GetAll()
        {
            var cadastros = await _unitOfWork.CadastroRepository.GetAllAsync();
            var dtos = cadastros.Select(c => _mapper.ToReadDTO(c));
            return Ok(dtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CadastroReadDTO>> GetById(int id)
        {
            var cadastro = await _unitOfWork.CadastroRepository.GetByIdAsync(id);
            if (cadastro == null)
            {
                _logger.LogWarning("Cadastro com ID {Id} não encontrado.", id);
                return NotFound(new { Message = $"Cadastro com ID {id} não encontrado." });
            }

            return Ok(_mapper.ToReadDTO(cadastro));
        }

        [HttpGet("cpf/{cpf}")]
        public async Task<ActionResult<CadastroReadDTO>> GetByCpf(string cpf)
        {
            var cadastros = await _unitOfWork.CadastroRepository.GetAllAsync();
            var cadastro = cadastros.FirstOrDefault(c => c.Cpf == cpf);

            if (cadastro == null)
            {
                _logger.LogWarning("Cadastro com CPF {Cpf} não encontrado.", cpf);
                return NotFound(new { Message = $"Cadastro com CPF {cpf} não encontrado." });
            }

            _logger.LogInformation("Cadastro encontrado por CPF: {Cpf}", cpf);
            return Ok(_mapper.ToReadDTO(cadastro));
        }

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

            _logger.LogInformation("Novo cadastro criado: {Cpf}", cadastro.Cpf);
            return CreatedAtAction(nameof(GetById), new { id = cadastro.Id }, _mapper.ToReadDTO(cadastro));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] CadastroCreateDTO dto)
        {
            var existing = await _unitOfWork.CadastroRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = $"Cadastro com ID {id} não encontrado." });

            _mapper.MapToEntity(dto, existing);
            await _unitOfWork.CadastroRepository.UpdateAsync(existing);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Cadastro atualizado: {Cpf}", existing.Cpf);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var cadastro = await _unitOfWork.CadastroRepository.GetByIdAsync(id);
            if (cadastro == null)
                return NotFound(new { Message = $"Cadastro com ID {id} não encontrado." });

            await _unitOfWork.CadastroRepository.RemoveAsync(id);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Cadastro removido: {Cpf}", cadastro.Cpf);
            return NoContent();
        }
    }
}
