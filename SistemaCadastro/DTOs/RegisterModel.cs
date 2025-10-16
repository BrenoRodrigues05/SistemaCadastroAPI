using System.ComponentModel.DataAnnotations;

namespace SistemaCadastro.DTOs
{
    /// <summary>
    /// Modelo para registro de um novo usuário.
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// Nome de usuário para cadastro.
        /// </summary>
        /// <example>usuario123</example>
        [Required(ErrorMessage = "O campo 'Username' é obrigatório.")]
        public string? Username { get; set; }

        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        /// <example>usuario@example.com</example>
        [Required(ErrorMessage = "O campo 'E-mail' é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo 'E-mail' deve ser um endereço de email válido.")]
        public string? Email { get; set; }

        /// <summary>
        /// Senha do usuário.
        /// </summary>
        /// <example>Senha@123</example>
        [Required(ErrorMessage = "O campo 'Password' é obrigatório.")]
        public string? Password { get; set; }
    }
}
