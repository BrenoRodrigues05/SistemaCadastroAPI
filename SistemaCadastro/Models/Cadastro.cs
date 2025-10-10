using SistemaCadastro.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCadastro.Models
{
    [Table("FuncionariosCadastrados")]
    public class Cadastro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(14, MinimumLength = 11, ErrorMessage = "O CPF deve ter entre 11 e 14 caracteres.")]
        [DisplayName("CPF")]
        [Cpf] // Validação personalizada para CPF
        public string? Cpf { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [PrimeiraLetraMaiúscula(ErrorMessage = "A primeira letra do nome deve ser maiúscula.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "Insira um endereçõ de e-mail válido")]
        [DisplayName("E-mail")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O Telefone é obrigatório")]
        [Phone(ErrorMessage = "Insira um telefone válido")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        [DisplayName("Data de Nascimento")]
        public DateOnly Nascimento { get; set; }

        [Required(ErrorMessage = "O Estado é obrigatório")]
        [PrimeiraLetraMaiúscula(ErrorMessage = "A primeira letra do nome deve ser maiúscula.")]
        public  string? Estado { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatória")]
        [PrimeiraLetraMaiúscula(ErrorMessage = "A primeira letra do nome deve ser maiúscula.")]
        public string? Cidade { get; set; }

        [Required(ErrorMessage = "Necessário inserir o Cargo")]
        [PrimeiraLetraMaiúscula(ErrorMessage = "A primeira letra do nome deve ser maiúscula.")]
        public string? Cargo { get; set; }
    }
}
