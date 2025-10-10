using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public CadastroController(IUnitOfWork unitOfWork, ILogger<CadastroController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
       

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cadastro>>> GetAll()
        {
            var cadastros = await _unitOfWork.CadastroRepository.GetAllAsync();
            return Ok(cadastros);
        }

        
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Cadastro>> GetById(int id)
        {
            var cadastro = await _unitOfWork.CadastroRepository.GetByIdAsync(id);
            if (cadastro == null)
            {
                _logger.LogWarning("Cadastro com ID {Id} não encontrado.", id);
                return NotFound(new { Message = $"Cadastro com ID {id} não encontrado." });
            }

            return Ok(cadastro);
        }

        [HttpGet("cpf/{Cpf}")]
        public async Task<ActionResult<Cadastro>> GetByCpf(string Cpf)
        {
            var cadastros = await _unitOfWork.CadastroRepository.GetAllAsync();
            var cadastro = cadastros.FirstOrDefault(c => c.Cpf == Cpf);

            if (cadastro == null)
            {
                _logger.LogWarning("Cadastro com CPF {Cpf} não encontrado.", Cpf);
                return NotFound(new { Message = $"Cadastro com CPF {Cpf} não encontrado." });
            }

            _logger.LogInformation("Cadastro encontrado por CPF: {Cpf}", Cpf);
            return Ok(cadastro);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Cadastro cadastro)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _unitOfWork.CadastroRepository.GetByCpf(cadastro.Cpf!))
                return BadRequest(new { Message = "CPF já cadastrado no sistema." });

            await _unitOfWork.CadastroRepository.AddAsync(cadastro);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Novo cadastro criado: {Cpf}", cadastro.Cpf);
            return CreatedAtAction(nameof(GetById), new { id = cadastro.Id }, cadastro);
        }

        
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] Cadastro cadastro)
        {
            if (id != cadastro.Id)
                return BadRequest(new { Message = "O ID do cadastro não corresponde ao informado na URL." });

            var existingCadastro = await _unitOfWork.CadastroRepository.GetByIdAsync(id);
            if (existingCadastro == null)
                return NotFound(new { Message = $"Cadastro com ID {id} não encontrado." });

            _unitOfWork.CadastroRepository.UpdateAsync(cadastro);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Cadastro atualizado: {Cpf}", cadastro.Cpf);
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

