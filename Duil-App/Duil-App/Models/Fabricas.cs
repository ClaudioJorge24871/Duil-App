using System.ComponentModel.DataAnnotations;

namespace Duil_App.Models
{
    /// <summary>
    /// Fábrica fornecedora de peças
    /// </summary>
    public class Fabricas : Empresas
    {

        /// <summary>
        /// Morada de descarga da fábrica
        /// </summary>
        [Display(Name = "Morada de Descarga")]
        [StringLength(100)]
        public string? MoradaDescarga { get; set; }

        /// <summary>
        /// Peças que a fabrica produz
        /// </summary>
        public ICollection<Pecas>? Pecas { get; set; }
    }
}
