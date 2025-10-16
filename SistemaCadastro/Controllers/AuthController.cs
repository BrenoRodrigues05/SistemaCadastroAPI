using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaCadastro.DTOs;
using SistemaCadastro.Models;
using SistemaCadastro.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SistemaCadastro.Controllers
{
    /// <summary>
    /// Controller para autenticação, registro de usuários, refresh de tokens e gerenciamento de roles.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Realiza login do usuário e retorna token de acesso e refresh token.
        /// </summary>
        /// <param name="model">Modelo de login com usuário e senha.</param>
        /// <returns>Token JWT de acesso, refresh token e data de expiração.</returns>
        /// <response code="200">Login realizado com sucesso.</response>
        /// <response code="401">Usuário ou senha inválidos.</response>
        /// <example>
        /// POST /api/Auth/login
        /// {
        ///   "username": "usuario123",
        ///   "password": "Senha@123"
        /// }
        /// </example>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username!);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password!))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
                var refreshToken = _tokenService.GenerateRefreshToken();

                int.TryParse(_configuration["Jwt:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);
                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);
                user.RefreshToken = refreshToken;

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    refreshToken = refreshToken
                });
            }

            return Unauthorized(new { message = "Usuário ou senha inválidos." });
        }

        /// <summary>
        /// Registra um novo usuário no sistema.
        /// </summary>
        /// <param name="model">Modelo de registro com username, email e senha.</param>
        /// <returns>Mensagem de sucesso ou lista de erros.</returns>
        /// <response code="200">Usuário criado com sucesso.</response>
        /// <response code="400">Usuário já existe.</response>
        /// <response code="500">Erro interno ao criar usuário.</response>
        /// <example>
        /// POST /api/Auth/register
        /// {
        ///   "username": "usuario123",
        ///   "email": "usuario@example.com",
        ///   "password": "Senha@123"
        /// }
        /// </example>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username!);
            if (userExists != null)
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Usuário já existe!" });

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
            };

            var result = await _userManager.CreateAsync(user, model.Password!);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Erro ao criar usuário.",
                    errors = result.Errors.Select(e => e.Description)
                });
            }

            return Ok(new { message = "Usuário criado com sucesso!" });
        }

        /// <summary>
        /// Atualiza tokens de acesso usando refresh token válido.
        /// </summary>
        /// <param name="tokenModel">Modelo contendo access token e refresh token atuais.</param>
        /// <returns>Novos tokens de acesso e refresh.</returns>
        /// <response code="200">Tokens atualizados com sucesso.</response>
        /// <response code="400">Token inválido ou expirado.</response>
        /// <example>
        /// POST /api/Auth/refresh-token
        /// {
        ///   "accessToken": "token_antigo",
        ///   "refreshToken": "refresh_token_antigo"
        /// }
        /// </example>
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModel tokenModel)
        {
            if (tokenModel == null)
                return BadRequest(new { message = "Token inválido." });

            var accessToken = tokenModel.AccessToken;
            var refreshToken = tokenModel.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);
            if (principal == null)
                return BadRequest(new { message = "Token inválido." });

            var username = principal.Identity?.Name;
            if (username == null)
                return BadRequest(new { message = "Token inválido." });

            var user = await _userManager.FindByNameAsync(username);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest(new { message = "Token inválido." });
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        /// <summary>
        /// Revoga o refresh token de um usuário específico. Somente administradores podem acessar.
        /// </summary>
        /// <param name="username">Nome do usuário cujo refresh token será revogado.</param>
        /// <returns>Mensagem indicando sucesso ou falha.</returns>
        /// <response code="200">Refresh token revogado com sucesso.</response>
        /// <response code="404">Usuário não encontrado.</response>
        /// <example>
        /// POST /api/Auth/revoke/usuario123
        /// </example>
        [Authorize(Roles = "Admin")]
        [HttpPost("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound(new { message = "Usuário não encontrado." });

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return Ok(new { message = "Refresh token revogado com sucesso." });
        }

        /// <summary>
        /// Cria uma nova role no sistema. Somente administradores podem acessar.
        /// </summary>
        /// <param name="roleName">Nome da role a ser criada.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        /// <response code="201">Role criada com sucesso.</response>
        /// <response code="400">Nome da role inválido ou já existente.</response>
        /// <response code="500">Erro ao criar role.</response>
        /// <example>
        /// POST /api/Auth/create-role?roleName=Admin
        /// </example>
        [HttpPost("create-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole([FromQuery] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest(new { message = "Nome da role não pode ser vazio." });

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (roleResult.Succeeded)
                {
                    _logger.LogInformation("Role {RoleName} criada com sucesso.", roleName);
                    return Created("", new { message = $"Role {roleName} criada com sucesso." });
                }
                else
                {
                    _logger.LogError("Erro ao criar role {RoleName}: {Errors}",
                        roleName, string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        message = $"Erro ao criar role {roleName}."
                    });
                }
            }

            return BadRequest(new { message = $"Role {roleName} já existe." });
        }

        /// <summary>
        /// Adiciona um usuário a uma role existente. Somente administradores podem acessar.
        /// </summary>
        /// <param name="email">Email do usuário a ser adicionado.</param>
        /// <param name="roleName">Nome da role.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        /// <response code="200">Usuário adicionado à role com sucesso.</response>
        /// <response code="400">Usuário não encontrado ou parâmetros inválidos.</response>
        /// <response code="500">Erro ao adicionar usuário à role.</response>
        /// <example>
        /// POST /api/Auth/add-user-to-role?email=usuario@example.com/roleName=Admin
        /// </example>
        [HttpPost("add-user-to-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserToRole([FromQuery] string email, [FromQuery] string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new { message = $"Usuário {email} não encontrado." });

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                _logger.LogInformation("Usuário {Email} adicionado à role {RoleName} com sucesso.", email, roleName);
                return Ok(new { message = $"Usuário {email} adicionado à role {roleName} com sucesso." });
            }
            else
            {
                _logger.LogError("Erro ao adicionar usuário {Email} à role {RoleName}: {Errors}",
                    email, roleName, string.Join(", ", result.Errors.Select(e => e.Description)));

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Erro ao adicionar usuário {email} à role {roleName}."
                });
            }
        }
    }
}
