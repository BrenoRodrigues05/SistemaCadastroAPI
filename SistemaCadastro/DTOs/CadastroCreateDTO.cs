using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SistemaCadastro.Models;
using SistemaCadastro.Validations;

namespace SistemaCadastro.DTOs
{
    public class CadastroCreateDTO
    {
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [Cpf]
        [DisplayName("CPF")]
        public string? Cpf { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [PrimeiraLetraMaiúscula]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Insira um endereço de e-mail válido.")]
        [DisplayName("E-mail")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "Insira um telefone válido.")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DisplayName("Data de Nascimento")]
        public DateOnly Nascimento { get; set; }

        [Required(ErrorMessage = "O estado é obrigatório.")]
        [PrimeiraLetraMaiúscula]
        public string? Estado { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatório.")]
        [PrimeiraLetraMaiúscula]
        public string? Cidade { get; set; }

        [PrimeiraLetraMaiúscula]

        [Required(ErrorMessage = "O cargo é obrigatório.")]
        public string? Cargo { get; set; }
    }
}
