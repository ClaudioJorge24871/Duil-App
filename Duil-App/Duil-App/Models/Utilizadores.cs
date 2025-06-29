using System.ComponentModel.DataAnnotations;
using Duil_App.Code;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Duil_App.Models
{

    /// <summary>
    /// utilizadores não anónimos da aplicação
    /// </summary>
    [Index(nameof(Nome), IsUnique = true)]
    [ValidaPorPais]
    public class Utilizadores: IdentityUser
    {
        /// <summary>
        /// Nome do utilizador
        /// </summary>
        [Display(Name = "Nome", ResourceType = typeof(Resources.Resource))]
        [StringLength(50)]
        [Required(ErrorMessageResourceName = "CampoObrigatorio", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string Nome { get; set; } = "";

        /// <summary>
        /// Morada do utilizador
        /// </summary>
        [Display(Name = "Morada", ResourceType = typeof(Resources.Resource))]
        [StringLength(50)]
        public string? Morada { get; set; }

        /// <summary>
        /// Código Postal da  morada do utilizador
        /// </summary>
        [Display(Name = "CodPostal", ResourceType = typeof(Resources.Resource))]
        [StringLength(50)]
        public string? CodPostal { get; set; }

        /// <summary>
        /// País da morada do utilizador
        /// </summary>
        [Display(Name = "Pais", ResourceType = typeof(Resources.Resource))]
        [StringLength(50)]
        [Required(ErrorMessageResourceName = "CampoObrigatorio", ErrorMessageResourceType = typeof(Resources.Resource))]
        public required string Pais { get; set; }

        /// <summary>
        /// Número de identificação fiscal do Utilizador
        /// </summary>
        [Display(Name = "NIF", ResourceType = typeof(Resources.Resource))]
        [StringLength(9)]
        [Required(ErrorMessageResourceName = "CampoObrigatorio", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string NIF { get; set; } = String.Empty;

        /// <summary>
        /// número de telemóvel do utilizador
        /// </summary>
        [Display(Name = "Telemovel", ResourceType = typeof(Resources.Resource))]
        [StringLength(18)]
        public string? Telemovel { get; set; }

        /* *************************
       * Definição dos relacionamentos
       * ************************** 
       */

        /// <summary>
        /// Lista das encomendas feitas pelo utilizador 
        /// </summary>
        public ICollection<Encomendas> ListaEncomendas { get; set; } = new
            HashSet<Encomendas>();
    }
}
