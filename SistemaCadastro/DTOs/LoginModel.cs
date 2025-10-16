using System.ComponentModel.DataAnnotations;

namespace SistemaCadastro.DTOs
{
    /// <summary>
    /// Modelo de login para autenticação de usuário.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Nome de usuário para login.
        /// </summary>
        /// <example>usuario123</example>
        [Required(ErrorMessage = "O campo 'Username' é obrigatório.")]
        public string? Username { get; set; }

        /// <summary>
        /// Senha do usuário.
        /// </summary>
        /// <example>Senha@123</example>
        [Required(ErrorMessage = "O campo 'Password' é obrigatório.")]
        public string? Password { get; set; }
    }
}
