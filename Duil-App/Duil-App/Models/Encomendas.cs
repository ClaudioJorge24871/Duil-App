using System.ComponentModel.DataAnnotations;

namespace Duil_App.Models
{
    /// <summary>
    /// Classe de encomendas realizadas na empresa
    /// </summary>
    public class Encomendas
    {
        /// <summary>
        /// Identificador da encomenda
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Identificador da encomenda do lado da empresa cliente
        /// </summary>
        public int? IdLadoCliente { get; set; }

        /// <summary>
        /// Data de realização da encomenda
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Total do preço da encomenda
        /// </summary>
        public decimal TotalPrecoUnit { get; set; }

        /// <summary>
        /// Quantidade total de peças na encomenda
        /// </summary>
        public int QuantidadeTotal { get; set; }

        /// <summary>
        /// Nome da transportadora que efetua a entrega
        /// </summary>
        public string? Transportadora { get; set; }

        /// <summary>
        /// Estado da encomenda
        /// </summary>
        public Estados Estado { get; set; }

        /// <summary>
        /// Identificação da empresa cliente 
        /// </summary>
        public int ClienteId { get; set; }
        public required Clientes Cliente { get; set; }

        
        public ICollection<LinhaEncomenda>? LinhasEncomenda { get; set; }
    }

    /// <summary>
    /// Estados das encomendas
    /// </summary>
    /// </summary>
    public enum Estados
    {
        Pendente,
        Confirmada,
        Concretizada,
        Cancelada
    }
}
