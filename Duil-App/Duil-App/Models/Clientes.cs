using System.ComponentModel.DataAnnotations;
using Duil_App.Resources;

namespace Duil_App.Models
{
    /// <summary>
    /// Empresa que realiza a encomenda
    /// </summary>
    public class Clientes : Empresas
    {

		/// <summary>
		/// Morada de carga da empresa cliente
		/// </summary>
		[Display(Name = "MoradaCarga", ResourceType = typeof(Resource))]

		[StringLength(255)]
        public string? MoradaCarga { get; set; }

        /// <summary>
        /// Lista de encomendas efetuadas pela empresa cliente
        /// </summary>
        public ICollection<Encomendas>? Encomendas { get; set; }
    }
}
