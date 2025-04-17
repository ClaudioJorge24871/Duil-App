using System.ComponentModel.DataAnnotations;

namespace Duil_App.Models
{
    /// <summary>
    /// Superclass the fabricas e clientes
    /// </summary>
    public abstract class Empresas
    {
        /// <summary>
        /// Nif/identificador da empresa
        /// </summary>
        [Key]
        public string Nif { get; set; }

        /// <summary>
        /// Nome da empresa
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// Morada sede da empresa
        /// </summary>
        public string? Morada { get; set; }

        /// <summary>
        /// Codigo postal da empresa
        /// </summary>
        public string? CodPostal { get; set; }

        /// <summary>
        /// País da empresa
        /// </summary>
        public string? Pais { get; set; }

        /// <summary>
        /// Telemovel de contacto da empresa
        /// </summary>
        public string? Telemovel { get; set; }

        /// <summary>
        /// Email da empresa
        /// </summary>
        public string? Email { get; set; }
    }
}
