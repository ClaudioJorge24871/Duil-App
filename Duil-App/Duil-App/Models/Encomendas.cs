using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Duil_App.Resources;
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
        [Display(Name = "OrdemDeEncomenda", ResourceType = typeof(Resources.Resource))]
        public required int IdLadoCliente { get; set; }

        /// <summary>
        /// Data de realização da encomenda
        /// </summary>
        [Display (Name = "Data", ResourceType = typeof(Resources.Resource))]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        /// <summary>
        /// Total do preço da encomenda 
        /// </summary>
        [Display(Name = "Valor", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessageResourceName = "CampoObrigatorio", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)] 
        public decimal TotalPrecoUnit { get; set; }

        /// <summary>
        /// Quantidade total de peças na encomenda
        /// </summary>
        [Display(Name = "Quantidade", ResourceType = typeof(Resources.Resource))]
        [Range(1, 1000000, ErrorMessage = "A quantidade total deve estar entre 1 e 999.999.")]
        [Required(ErrorMessageResourceName = "CampoObrigatorio",
        ErrorMessageResourceType = typeof(Resources.Resource))]
        public int QuantidadeTotal { get; set; }

        /// <summary>
        /// Nome da transportadora que efetua a entrega
        /// </summary>
        [Display(Name = "EmpresaTrans", ResourceType = typeof(Resources.Resource))]
        [StringLength(100)]
        public string? Transportadora { get; set; } = string.Empty;  

        /// <summary>
        /// Estado da encomenda
        /// </summary>
        [Required(ErrorMessageResourceName = "CampoObrigatorio",
        ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "Estado", ResourceType = typeof(Resources.Resource))]
        public Estados Estado { get; set; }

        /// <summary>
        /// Identificação do cliente 
        /// </summary>
        [Display (Name = "Cliente", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessageResourceName = "CampoObrigatorio",
        ErrorMessageResourceType = typeof(Resources.Resource))]
        public required string ClienteId { get; set; } 

        [ValidateNever]
        public required Clientes Cliente { get; set; }


        public ICollection<LinhaEncomenda> LinhasEncomenda { get; set; } = new List<LinhaEncomenda>();
    }

    /// <summary>
    /// Estados das encomendas
    /// </summary>
    public enum Estados
    {
        Pendente,
        Confirmada,
        Concretizada,
        Cancelada
    }
}
