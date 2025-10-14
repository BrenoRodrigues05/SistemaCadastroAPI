using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaCadastro.DTOs
{
    /// <summary>
    /// Representa os dados retornados ao consultar um cadastro existente no sistema.
    /// </summary>
    /// <remarks>
    /// Este DTO é utilizado nas operações de leitura (<c>GET</c>) e exibe as informações completas de um cadastro.
    /// Nenhum campo possui validação de entrada, pois se trata apenas de dados de saída.
    /// </remarks>
    public class CadastroReadDTO
    {
        /// <summary>
        /// Identificador único do cadastro.
        /// </summary>
        /// <remarks>
        /// Gerado automaticamente pelo sistema no momento da criação do registro.
        /// </remarks>
        /// <example>101</example>
        public int Id { get; set; }

        /// <summary>
        /// CPF do usuário cadastrado.
        /// </summary>
        /// <remarks>
        /// Valor armazenado em formato numérico, sem pontos nem traços.
        /// </remarks>
        /// <example>12345678909</example>
        public string? Cpf { get; set; }

        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        /// <remarks>
        /// Armazenado com a primeira letra maiúscula conforme as validações aplicadas durante o cadastro.
        /// </remarks>
        /// <example>Lucas Pereira</example>
        public string? Nome { get; set; }

        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        /// <remarks>
        /// Exibido conforme informado no momento do cadastro, podendo ser utilizado para comunicação.
        /// </remarks>
        /// <example>lucas.pereira@email.com</example>
        public string? Email { get; set; }

        /// <summary>
        /// Número de telefone do usuário.
        /// </summary>
        /// <remarks>
        /// Pode incluir o DDD e ser exibido com ou sem formatação.
        /// </remarks>
        /// <example>(11) 98765-4321</example>
        public string? Telefone { get; set; }

        /// <summary>
        /// Data de nascimento do usuário.
        /// </summary>
        /// <remarks>
        /// Armazenada no formato ISO (YYYY-MM-DD).
        /// </remarks>
        /// <example>1992-03-10</example>
        public DateOnly Nascimento { get; set; }

        /// <summary>
        /// Estado de residência do usuário.
        /// </summary>
        /// <remarks>
        /// Exibe o estado informado durante o cadastro, com a primeira letra maiúscula.
        /// </remarks>
        /// <example>Rio de Janeiro</example>
        public string? Estado { get; set; }

        /// <summary>
        /// Cidade de residência do usuário.
        /// </summary>
        /// <remarks>
        /// Exibe a cidade associada ao cadastro, armazenada conforme validação de entrada.
        /// </remarks>
        /// <example>Niterói</example>
        public string? Cidade { get; set; }

        /// <summary>
        /// Cargo ou função do usuário na empresa.
        /// </summary>
        /// <remarks>
        /// Valor textual informativo que representa o papel ou posição do usuário.
        /// </remarks>
        /// <example>Coordenador de TI</example>
        public string? Cargo { get; set; }
    }
}
