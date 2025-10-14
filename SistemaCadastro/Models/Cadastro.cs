using SistemaCadastro.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCadastro.Models
{
    /// <summary>
    /// Representa um funcionário cadastrado no sistema.
    /// </summary>
    /// <remarks>
    /// A classe <see cref="Cadastro"/> corresponde à tabela <c>FuncionariosCadastrados</c> no banco de dados
    /// e armazena informações pessoais e profissionais dos colaboradores.
    /// <para>
    /// Inclui validações de integridade via <see cref="DataAnnotations"/>, como obrigatoriedade de campos,
    /// tamanho máximo de strings e regras personalizadas implementadas através de 
    /// <see cref="CpfAttribute"/> e <see cref="PrimeiraLetraMaiúsculaAttribute"/>.
    /// </para>
    /// </remarks>
    [Table("FuncionariosCadastrados")]
    public class Cadastro
    {
        /// <summary>
        /// Identificador único do funcionário cadastrado.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Número de CPF do funcionário.
        /// </summary>
        /// <remarks>
        /// Deve conter apenas dígitos e opcionalmente separadores (pontos e hífen),
        /// totalizando entre 11 e 14 caracteres.
        /// A validação é feita pelo atributo personalizado <see cref="CpfAttribute"/>.
        /// </remarks>
        /// <example>Exemplo válido: <c>123.456.789-09</c></example>
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(14, MinimumLength = 11, ErrorMessage = "O CPF deve ter entre 11 e 14 caracteres.")]
        [DisplayName("CPF")]
        [Cpf]
        public string? Cpf { get; set; }

        /// <summary>
        /// Nome completo do funcionário.
        /// </summary>
        /// <remarks>
        /// A primeira letra deve ser maiúscula, conforme a validação de <see cref="PrimeiraLetraMaiúsculaAttribute"/>.
        /// </remarks>
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [PrimeiraLetraMaiúscula(ErrorMessage = "A primeira letra do nome deve ser maiúscula.")]
        public string? Nome { get; set; }

        /// <summary>
        /// Endereço de e-mail do funcionário.
        /// </summary>
        /// <remarks>
        /// O formato é validado através do atributo <see cref="EmailAddressAttribute"/>.
        /// </remarks>
        [Required(ErrorMessage = "O E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "Insira um endereço de e-mail válido.")]
        [DisplayName("E-mail")]
        public string? Email { get; set; }

        /// <summary>
        /// Número de telefone do funcionário.
        /// </summary>
        /// <remarks>
        /// Deve seguir um formato válido conforme o atributo <see cref="PhoneAttribute"/>.
        /// </remarks>
        [Required(ErrorMessage = "O Telefone é obrigatório")]
        [Phone(ErrorMessage = "Insira um telefone válido")]
        public string? Telefone { get; set; }

        /// <summary>
        /// Data de nascimento do funcionário.
        /// </summary>
        /// <remarks>
        /// Utiliza o tipo <see cref="DateOnly"/> para armazenar apenas a data, sem informações de hora.
        /// </remarks>
        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        [DisplayName("Data de Nascimento")]
        public DateOnly Nascimento { get; set; }

        /// <summary>
        /// Estado de residência do funcionário.
        /// </summary>
        /// <remarks>
        /// A primeira letra deve ser maiúscula, conforme o atributo <see cref="PrimeiraLetraMaiúsculaAttribute"/>.
        /// </remarks>
        [Required(ErrorMessage = "O Estado é obrigatório")]
        [PrimeiraLetraMaiúscula(ErrorMessage = "A primeira letra do nome deve ser maiúscula.")]
        public string? Estado { get; set; }

        /// <summary>
        /// Cidade de residência do funcionário.
        /// </summary>
        /// <remarks>
        /// A validação <see cref="PrimeiraLetraMaiúsculaAttribute"/> assegura formatação adequada.
        /// </remarks>
        [Required(ErrorMessage = "A cidade é obrigatória")]
        [PrimeiraLetraMaiúscula(ErrorMessage = "A primeira letra do nome deve ser maiúscula.")]
        public string? Cidade { get; set; }

        /// <summary>
        /// Cargo ocupado pelo funcionário na empresa.
        /// </summary>
        /// <remarks>
        /// Este campo é obrigatório e deve começar com letra maiúscula.
        /// </remarks>
        [Required(ErrorMessage = "Necessário inserir o Cargo")]
        [PrimeiraLetraMaiúscula(ErrorMessage = "A primeira letra do nome deve ser maiúscula.")]
        public string? Cargo { get; set; }
    }
}
