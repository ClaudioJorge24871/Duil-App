using System.ComponentModel.DataAnnotations;

namespace Duil_App.Models
{
    /// <summary>
    /// Superclass the fabricas e clientes
    /// </summary>
    public abstract class Empresas : IValidatableObject
    {
        /// <summary>
        /// Nif/identificador da empresa
        /// </summary>
        [Key]
        [Display(Name = "NIF")]
        public string Nif { get; set; }

        /// <summary>
        /// Nome da empresa
        /// </summary>
        [Display(Name = "Nome")]
        public string? Nome { get; set; }

        /// <summary>
        /// Morada sede da empresa
        /// </summary>
        [Display(Name = "Morada")]
        public string? Morada { get; set; }

        /// <summary>
        /// Codigo postal da empresa
        /// </summary>
        [Display(Name = "Código Postal")]
        public string? CodPostal { get; set; }

        /// <summary>
        /// País da empresa
        /// </summary>
        [Display(Name = "País")]
        public string? Pais { get; set; }

        /// <summary>
        /// Telemovel de contacto da empresa
        /// </summary>
        [Display(Name = "Telemóvel")]
        public string? Telemovel { get; set; }  

        /// <summary>
        /// Email da empresa
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Nif))
            { 
                yield return new ValidationResult("O NIF é obrigatório.", new[] { nameof(Nif) });
                yield break;
            }

            switch (Pais?.ToLowerInvariant())
            {
                case "portugal":
                    if (!System.Text.RegularExpressions.Regex.IsMatch(Nif, @"^[1-9][0-9]{8}$"))
                        yield return new ValidationResult("O NIF deve conter 9 digitos entre 1 e 9", new[] { nameof(Nif) });
                    break;

            }
        }
    }
}
