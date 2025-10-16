using Microsoft.AspNetCore.Identity;

namespace SistemaCadastro.Models
{
    /// <summary>
    /// Representa um usuário do sistema com suporte a autenticação e gerenciamento de tokens.
    /// Herda da classe <see cref="IdentityUser"/>.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Token de atualização (refresh token) associado ao usuário.
        /// </summary>
        /// <example>c29tZS1yYW5kb20tcmVmcmVzaC10b2tlbg==</example>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Data e hora de expiração do <see cref="RefreshToken"/>.
        /// </summary>
        /// <example>2025-12-31T23:59:59</example>
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
