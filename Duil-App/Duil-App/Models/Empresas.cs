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
        [StringLength(50)]
        public string? Nome { get; set; }

        /// <summary>
        /// Morada sede da empresa
        /// </summary>
        [Display(Name = "Morada")]
        [StringLength(100)]
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
        [StringLength(50)]
        [Required(ErrorMessage = "O {0} é obrigatório")]
        public string Pais { get; set; }

        /// <summary>
        /// Telemovel de contacto da empresa
        /// </summary>
        [Display(Name = "Telemóvel")]
        [Required(ErrorMessage = "O {0} é obrigatório")]
        public string Telemovel { get; set; }

        /// <summary>
        /// Email da empresa
        /// </summary>
        [Display(Name = "Email")]
        public string? Email { get; set; }

        /// <summary>
        /// Validação do NIF e telemóvel por País
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

            switch (Pais.ToLowerInvariant())
            {
                case "portugal":
                    //Validação do NIF portugûes
                    if (!System.Text.RegularExpressions.Regex.IsMatch(Nif, @"^[1-9][0-9]{8}$"))
                        yield return new ValidationResult("O NIF deve ser válido", new[] { nameof(Nif) });

                    //Validação do telemóvel português
                    if(!System.Text.RegularExpressions.Regex.IsMatch(Telemovel, @"^9[1236][0-9]{7}$"))
                        yield return new ValidationResult("O Telemóvel deve ser válido", new[] {nameof (Telemovel) });

                    //Validação do Código Postal Português
                    if (!string.IsNullOrWhiteSpace(CodPostal) &&
                        !System.Text.RegularExpressions.Regex.IsMatch(CodPostal, @"^[1-9][0-9]{3}-[0-9]{3}$"))
                        yield return new ValidationResult("O Código Postal deve ser válido.", new[] { nameof(CodPostal) });
                    break;

            }
        }
    }
}
