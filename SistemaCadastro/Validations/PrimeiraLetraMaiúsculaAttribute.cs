using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaCadastro.Models
{
    /// <summary>
    /// Validação personalizada que garante que a primeira letra de uma string seja maiúscula.
    /// </summary>
    /// <remarks>
    /// Pode ser aplicada a propriedades do tipo <see cref="string"/> em DTOs ou entidades.
    /// Retorna uma mensagem de erro padrão se a primeira letra não estiver em maiúscula.
    /// </remarks>
    public class PrimeiraLetraMaiúsculaAttribute : ValidationAttribute
    {
        /// <summary>
        /// Verifica se a primeira letra do valor é maiúscula.
        /// </summary>
        /// <param name="value">Valor da propriedade a ser validada.</param>
        /// <param name="validationContext">Contexto da validação.</param>
        /// <returns>
        /// <see cref="ValidationResult.Success"/> se válido; caso contrário,
        /// uma instância de <see cref="ValidationResult"/> com a mensagem de erro.
        /// </returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var primeiraLetra = value.ToString()[0].ToString();
            if (primeiraLetra != primeiraLetra.ToUpper())
            {
                return new ValidationResult("A primeira letra precisa ser maiúscula");
            }

            return ValidationResult.Success;
        }
    }
}
