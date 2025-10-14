using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SistemaCadastro.Validations
{
    /// <summary>
    /// Validação personalizada para verificar se um valor corresponde a um CPF válido.
    /// </summary>
    /// <remarks>
    /// Este atributo pode ser aplicado a propriedades de tipo <see cref="string"/> em DTOs ou entidades.
    /// Ele valida:
    /// <list type="bullet">
    /// <item>Se o valor não é nulo.</item>
    /// <item>Se possui 11 dígitos numéricos.</item>
    /// <item>Se não é um CPF com todos os dígitos iguais.</item>
    /// <item>Se os dígitos verificadores estão corretos.</item>
    /// </list>
    /// </remarks>
    public class CpfAttribute : ValidationAttribute
    {
        /// <summary>
        /// Inicializa uma nova instância do <see cref="CpfAttribute"/> com a mensagem de erro padrão.
        /// </summary>
        public CpfAttribute()
        {
            ErrorMessage = "CPF inválido.";
        }

        /// <summary>
        /// Verifica se o valor fornecido é um CPF válido.
        /// </summary>
        /// <param name="value">O valor da propriedade a ser validada.</param>
        /// <returns><c>true</c> se o CPF for válido; caso contrário, <c>false</c>.</returns>
        public override bool IsValid(object? value)
        {
            if (value == null)
                return false;

            // Remove pontos, traços e espaços
            string cpf = value.ToString()!.Replace(".", "").Replace("-", "").Trim();

            // Deve ter 11 dígitos e ser numérico
            if (cpf.Length != 11 || !long.TryParse(cpf, out _))
                return false;

            // Descarta CPFs com todos os dígitos iguais (ex: 11111111111)
            if (cpf.Distinct().Count() == 1)
                return false;

            // Validação dos dígitos verificadores
            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            string digito = resto.ToString();

            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}
