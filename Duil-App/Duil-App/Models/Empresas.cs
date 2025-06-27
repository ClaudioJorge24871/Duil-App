using System.ComponentModel.DataAnnotations;
using Duil_App.Code;

namespace Duil_App.Models
{
    /// <summary>
    /// Superclass the fabricas e clientes
    /// </summary>
    [ValidaPorPais]
    public abstract class Empresas
    {
        /// <summary>
        /// Nif/identificador da empresa
        /// </summary>
        [Key]
        [Display(Name = "NIF")]
        [Required(ErrorMessage = "O {0} é um campo obrigatório")]
        public required string Nif { get; set; }

        /// <summary>
        /// Nome da empresa
        /// </summary>
        [Display(Name = "Nome")]
        [StringLength(50)]
        [Required(ErrorMessage = "O {0} do cliente é um campo obrigatório")]
        public required string Nome { get; set; } 

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
        public required string Pais { get; set; } 

        /// <summary>
        /// Telemovel de contacto da empresa
        /// </summary>
        [Display(Name = "Telemóvel")]
        public string? Telemovel { get; set; }

        /// <summary>
        /// Email da empresa
        /// </summary>
        [Display(Name = "Email")]
        public string? Email { get; set; }

        
    }
}
