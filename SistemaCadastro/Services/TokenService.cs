using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SistemaCadastro.Services
{
    /// <summary>
    /// Serviço responsável por gerar tokens JWT de acesso e refresh tokens, e validar tokens expirados.
    /// </summary>
    public class TokenService : ITokenService
    {
        /// <summary>
        /// Gera um token de acesso JWT com base nas claims fornecidas e nas configurações do sistema.
        /// </summary>
        /// <param name="claims">Lista de claims do usuário.</param>
        /// <param name="_config">Configurações do sistema (<see cref="IConfiguration"/>), incluindo secret key, issuer e audience.</param>
        /// <returns>Um <see cref="JwtSecurityToken"/> representando o token de acesso.</returns>
        /// <exception cref="ArgumentNullException">Se a secret key estiver ausente ou inválida.</exception>
        public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config)
        {
            var key = _config["Jwt:SecretKey"] ?? throw new ArgumentNullException("SecretKey inválida");

            var privateKey = Encoding.UTF8.GetBytes(key);
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(privateKey),
                SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(
                    double.Parse(_config["Jwt:TokenValidityInMinutes"] ?? "10")),
                Audience = _config["Jwt:ValidAudience"],
                Issuer = _config["Jwt:ValidIssuer"],
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return token;
        }

        /// <summary>
        /// Gera um refresh token seguro e aleatório.
        /// </summary>
        /// <returns>Uma string representando o refresh token.</returns>
        public string GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[128];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(secureRandomBytes);

            var refreshToken = Convert.ToBase64String(secureRandomBytes);
            return refreshToken;
        }

        /// <summary>
        /// Obtém as claims de um token JWT expirado, sem validar seu tempo de expiração.
        /// Útil para operações de refresh token.
        /// </summary>
        /// <param name="token">Token JWT expirado.</param>
        /// <param name="_config">Configurações do sistema (<see cref="IConfiguration"/>), incluindo secret key.</param>
        /// <returns>Um <see cref="ClaimsPrincipal"/> representando o usuário contido no token.</returns>
        /// <exception cref="ArgumentNullException">Se a secret key estiver ausente ou inválida.</exception>
        /// <exception cref="SecurityTokenException">Se o token for inválido ou não estiver assinado corretamente.</exception>
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config)
        {
            var secretKey = _config["Jwt:SecretKey"] ?? throw new ArgumentNullException("SecretKey inválida");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = false // Não valida expiração
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

            if (validatedToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token inválido");
            }

            return principal;
        }
    }
}
