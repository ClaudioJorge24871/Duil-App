using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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
        [Display(Name = "Ordem de Encomenda")]
        public int? IdLadoCliente { get; set; }

        /// <summary>
        /// Data de realização da encomenda
        /// </summary>
        [Display (Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        /// <summary>
        /// Total do preço da encomenda 
        /// </summary>
        [Display(Name = "Valor")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço total deve ser maior que zero.")]
        [Required(ErrorMessage ="O {0} total é um campo obrigatório")]
        public decimal TotalPrecoUnit { get; set; }

        /// <summary>
        /// Quantidade total de peças na encomenda
        /// </summary>
        [Display(Name = "Quantidade")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade total deve ser maior que zero.")]
        [Required(ErrorMessage = "A {0} total é um campo obrigatório.")]
        public int QuantidadeTotal { get; set; }

        /// <summary>
        /// Nome da transportadora que efetua a entrega
        /// </summary>
        [Display(Name = "Empresa transportadora")]
        [StringLength(100)]
        public string? Transportadora { get; set; } = string.Empty;  

        /// <summary>
        /// Estado da encomenda
        /// </summary>
        [Required(ErrorMessage = "O estado da encomenda é obrigatório")]
        public Estados Estado { get; set; }

        /// <summary>
        /// Identificação do cliente 
        /// </summary>
        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "O {0} é um campo obrigatório.")]
        public required string ClienteId { get; set; } 

        [ValidateNever]
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
