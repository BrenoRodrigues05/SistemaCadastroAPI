using SistemaCadastro.Models;
using SistemaCadastro.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaCadastro.DTOs
{
    /// <summary>
    /// Representa os dados que podem ser atualizados parcialmente em um cadastro existente.
    /// </summary>
    /// <remarks>
    /// Este DTO é utilizado nas operações <c>PATCH</c> ou <c>PUT parcial</c>, permitindo
    /// a atualização de um ou mais campos do registro sem a necessidade de reenviar todos os dados.
    /// Nenhum campo é obrigatório, pois o objetivo é a atualização seletiva.
    /// </remarks>
    public class CadastroPatchDTO
    {
        /// <summary>
        /// CPF do usuário a ser atualizado.
        /// </summary>
        /// <remarks>
        /// Pode ser informado apenas se o CPF precisar ser alterado.  
        /// Deve obedecer ao formato brasileiro e ser válido conforme a validação personalizada <see cref="CpfAttribute"/>.
        /// </remarks>
        /// <example>12345678909</example>
        public string? Cpf { get; set; }

        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        /// <remarks>
        /// Deve começar com letra maiúscula e pode conter espaços e acentuação.
        /// </remarks>
        /// <example>Maria Fernanda Souza</example>
        public string? Nome { get; set; }

        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        /// <remarks>
        /// Deve ser um endereço de e-mail válido e único dentro do sistema.
        /// </remarks>
        /// <example>maria.souza@empresa.com</example>
        public string? Email { get; set; }

        /// <summary>
        /// Número de telefone do usuário.
        /// </summary>
        /// <remarks>
        /// Pode incluir o DDD e ser informado com ou sem formatação.
        /// Exemplo de formato aceito: (11) 91234-5678.
        /// </remarks>
        /// <example>(11) 99876-5432</example>
        public string? Telefone { get; set; }

        /// <summary>
        /// Data de nascimento do usuário.
        /// </summary>
        /// <remarks>
        /// Caso informada, deve representar uma data válida anterior à data atual.
        /// </remarks>
        /// <example>1990-04-15</example>
        public DateOnly Nascimento { get; set; }

        /// <summary>
        /// Estado de residência do usuário.
        /// </summary>
        /// <remarks>
        /// Deve ter a primeira letra maiúscula e conter apenas caracteres alfabéticos.
        /// </remarks>
        /// <example>Minas Gerais</example>
        public string? Estado { get; set; }

        /// <summary>
        /// Cidade de residência do usuário.
        /// </summary>
        /// <remarks>
        /// Deve ter a primeira letra maiúscula e conter apenas caracteres alfabéticos.
        /// </remarks>
        /// <example>Belo Horizonte</example>
        public string? Cidade { get; set; }

        /// <summary>
        /// Cargo ou função atual do usuário.
        /// </summary>
        /// <remarks>
        /// Representa o cargo atualizado no sistema.  
        /// Deve começar com letra maiúscula e não conter números.
        /// </remarks>
        /// <example>Gerente de Projetos</example>
        public string? Cargo { get; set; }
    }
}
