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
        /// Identificador da encomenda do lado da empresa cliente [ordem de encomenda]
        /// </summary>
        [Display(Name = "OE")]
        public int? IdLadoCliente { get; set; }

        /// <summary>
        /// Data de realização da encomenda
        /// </summary>
        [Display (Name = "Data")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "01/01/2000", "01/01/2099", ErrorMessage = "A data deve estar entre 01/01/2000 e 01/01/2099.")]
        public DateTime Data { get; set; }

        /// <summary>
        /// Total do preço da encomenda 
        /// </summary>
        [Display(Name = "Valor")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço total deve ser maior que zero.")]
        public decimal TotalPrecoUnit { get; set; }

        /// <summary>
        /// Quantidade total de peças na encomenda
        /// </summary>
        [Display(Name = "Quantidade")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade total deve ser maior que zero.")]
        public int QuantidadeTotal { get; set; }

        /// <summary>
        /// Nome da transportadora que efetua a entrega
        /// </summary>
        [Display(Name = "Empresa transportadora")]
        [StringLength(100)]
        public string? Transportadora { get; set; }

        /// <summary>
        /// Estado da encomenda
        /// </summary>
        public Estados Estado { get; set; }

        /// <summary>
        /// Identificação do cliente 
        /// </summary>
        [Required(ErrorMessage = "O cliente é obrigatório.")]
        [Display(Name = "Cliente")]
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
