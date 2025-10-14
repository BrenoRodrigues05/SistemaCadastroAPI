using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SistemaCadastro.Models;
using SistemaCadastro.Validations;

namespace SistemaCadastro.DTOs
{
    /// <summary>
    /// Representa os dados necessários para criar um novo registro de cadastro no sistema.
    /// </summary>
    /// <remarks>
    /// Esta classe é utilizada como DTO (Data Transfer Object) para entrada de dados na operação de criação.
    /// Inclui validações automáticas via <see cref="DataAnnotations"/> e validações personalizadas.
    /// </remarks>
    public class CadastroCreateDTO
    {
        /// <summary>
        /// CPF do usuário a ser cadastrado.
        /// </summary>
        /// <remarks>
        /// Deve ser um CPF válido no formato brasileiro, sem necessidade de pontuação.
        /// </remarks>
        /// <example>12345678909</example>
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [Cpf]
        [DisplayName("CPF")]
        public string? Cpf { get; set; }

        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        /// <remarks>
        /// A primeira letra deve ser maiúscula. 
        /// O nome não pode ser nulo nem vazio.
        /// </remarks>
        /// <example>João da Silva</example>
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [PrimeiraLetraMaiúscula]
        public string? Nome { get; set; }

        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        /// <remarks>
        /// Deve ser um e-mail válido e único no sistema.
        /// </remarks>
        /// <example>joao.silva@email.com</example>
        [Required(ErrorMessage = "O E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Insira um endereço de e-mail válido.")]
        [DisplayName("E-mail")]
        public string? Email { get; set; }

        /// <summary>
        /// Número de telefone do usuário.
        /// </summary>
        /// <remarks>
        /// Deve conter apenas dígitos e, opcionalmente, o DDD.
        /// Exemplo de formato aceito: (11) 91234-5678.
        /// </remarks>
        /// <example>(11) 91234-5678</example>
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "Insira um telefone válido.")]
        public string? Telefone { get; set; }

        /// <summary>
        /// Data de nascimento do usuário.
        /// </summary>
        /// <remarks>
        /// É obrigatória e deve representar uma data válida anterior à data atual.
        /// </remarks>
        /// <example>1995-08-20</example>
        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DisplayName("Data de Nascimento")]
        public DateOnly Nascimento { get; set; }

        /// <summary>
        /// Estado de residência do usuário.
        /// </summary>
        /// <remarks>
        /// Deve ter a primeira letra maiúscula e conter apenas letras.
        /// </remarks>
        /// <example>São Paulo</example>
        [Required(ErrorMessage = "O estado é obrigatório.")]
        [PrimeiraLetraMaiúscula]
        public string? Estado { get; set; }

        /// <summary>
        /// Cidade de residência do usuário.
        /// </summary>
        /// <remarks>
        /// Deve ter a primeira letra maiúscula e conter apenas letras.
        /// </remarks>
        /// <example>Campinas</example>
        [Required(ErrorMessage = "A cidade é obrigatório.")]
        [PrimeiraLetraMaiúscula]
        public string? Cidade { get; set; }

        /// <summary>
        /// Cargo ou função do usuário dentro da empresa.
        /// </summary>
        /// <remarks>
        /// Deve ter a primeira letra maiúscula e não pode ser nulo.
        /// </remarks>
        /// <example>Analista de Sistemas</example>
        [Required(ErrorMessage = "O cargo é obrigatório.")]
        [PrimeiraLetraMaiúscula]
        public string? Cargo { get; set; }
    }
}
