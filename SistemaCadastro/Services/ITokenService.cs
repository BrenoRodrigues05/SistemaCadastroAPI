using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SistemaCadastro.Services
{
    /// <summary>
    /// Define métodos para geração e validação de tokens de autenticação JWT e refresh tokens.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Gera um token de acesso JWT com base em uma lista de claims e configurações do sistema.
        /// </summary>
        /// <param name="claims">Lista de claims do usuário.</param>
        /// <param name="configuration">Objeto de configuração (<see cref="IConfiguration"/>), contendo chaves e tempo de expiração.</param>
        /// <returns>Um <see cref="JwtSecurityToken"/> contendo as informações de autenticação.</returns>
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration configuration);

        /// <summary>
        /// Gera um refresh token aleatório e seguro.
        /// </summary>
        /// <returns>Uma string representando o refresh token.</returns>
        string GenerateRefreshToken();

        /// <summary>
        /// Obtém o <see cref="ClaimsPrincipal"/> de um token expirado, útil para renovar tokens.
        /// </summary>
        /// <param name="token">Token JWT expirado.</param>
        /// <param name="configuration">Objeto de configuração (<see cref="IConfiguration"/>), contendo a chave de assinatura.</param>
        /// <returns>O <see cref="ClaimsPrincipal"/> associado ao token expirado, ou <c>null</c> se o token for inválido.</returns>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration configuration);
    }
}
