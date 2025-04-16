using System.ComponentModel.DataAnnotations;

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
        public string MoradaCarga { get; set; }

        /// <summary>
        /// Lista de encomendas efetuadas pela empresa cliente
        /// </summary>
        public ICollection<Encomendas> Encomendas { get; set; }
    }
}
