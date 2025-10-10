using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaCadastro.DTOs
{
    public class CadastroReadDTO
    {
        public string? Cpf { get; set; }
        public string? Nome { get; set; }

        public string? Email { get; set; }

        public string? Telefone { get; set; }

        public DateOnly Nascimento { get; set; }

        public string? Estado { get; set; }

        public string? Cidade { get; set; }

        public string? Cargo { get; set; }
    }
}
