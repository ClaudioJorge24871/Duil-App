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
        [Display(Name = "Nome")]
        [StringLength(50)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string Nome { get; set; } = "";

        /// <summary>
        /// Morada do utilizador
        /// </summary>
        [Display(Name = "Morada")]
        [StringLength(50)]
        public string? Morada { get; set; }

        /// <summary>
        /// Código Postal da  morada do utilizador
        /// </summary>
        [Display(Name = "Código Postal")]
        [StringLength(50)]
        public string? CodPostal { get; set; }

        /// <summary>
        /// País da morada do utilizador
        /// </summary>
        [Display(Name = "País")]
        [StringLength(50)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public required string Pais { get; set; }

        /// <summary>
        /// Número de identificação fiscal do Utilizador
        /// </summary>
        [Display(Name = "NIF")]
        [StringLength(9)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string NIF { get; set; } = String.Empty;

        /// <summary>
        /// número de telemóvel do utilizador
        /// </summary>
        [Display(Name = "Telemóvel")]
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
