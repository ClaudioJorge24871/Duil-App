using System.ComponentModel.DataAnnotations;

namespace Duil_App.Models
{
    /// <summary>
    /// Utilizadores da aplicação, podem ser empresas ou fábricas
    /// </summary>
    public abstract class Clientes
    {
        /// <summary>
        /// Identificador do cliente
        /// </summary>
        [Key]
        public int NIF { get; set; }

        /// <summary>
        /// Nome do cliente
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Morada do cliente (sede)
        /// </summary>
        public string Morada { get; set; }

        /// <summary>
        /// Codigo postal do cliente (sede)
        /// </summary>
        public string codPostal { get; set; }

        /// <summary>
        /// Pais do cliente
        /// </summary>
        public string Pais { get; set; }

        /// <summary>
        /// Número de telemovel do cliente
        /// </summary>
        public string Telemovel { get; set; }

        /// <summary>
        /// Email da empresa ou fábrica
        /// </summary>
        public string email { get; set; }

        

    }
}
