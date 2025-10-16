namespace SistemaCadastro.DTOs
{
    /// <summary>
    /// Representa os tokens de autenticação de um usuário.
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// Token de acesso JWT (JSON Web Token) válido para autenticação.
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
        public string? AccessToken { get; set; }

        /// <summary>
        /// Token de atualização (refresh token) usado para renovar o token de acesso.
        /// </summary>
        /// <example>c29tZS1yYW5kb20tcmVmcmVzaC10b2tlbg==</example>
        public string? RefreshToken { get; set; }
    }
}
