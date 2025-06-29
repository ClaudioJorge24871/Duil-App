using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Duil_App.Models
{
    /// <summary>
    /// Pecas que fazem parte de uma encomenda. 
    /// Possui referência propria, preço, designação e identificaçaõ da fábrica que produz
    /// </summary>
    public class Pecas
    {
        /// <summary>
        /// Identificação da peça
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Referência da peça
        /// </summary>
        /// 
        [Display(Name = "referenciaPEca", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessageResourceName = "CampoObrigatorio", ErrorMessageResourceType = typeof(Resources.Resource))]
        public int Referencia { get; set; }

        /// <summary>
        /// Designação da Peça. Simples nome ou descrição
        /// </summary>
        [StringLength(100)]
        [Display(Name = "Designacao", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessageResourceName = "CampoObrigatorio", ErrorMessageResourceType = typeof(Resources.Resource))]
        public required string Designacao { get; set; }

        /// <summary>
        /// Preco unitário
        /// </summary>
        [Display(Name = "PrecoUnit", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessageResourceName = "CampoObrigatorio", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Column(TypeName = "decimal(18, 2)")]
        [RegularExpression(@"^\d{1,7}([.,]\d{1,3})?$", ErrorMessage = "Máximo de 10 dígitos, até 3 casas decimais.")]
        public decimal PrecoUnit { get; set; }

        /// <summary>
        /// Fabricante
        /// </summary>
        [Required(ErrorMessageResourceName = "CampoObrigatorio", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "Fabrica", ResourceType = typeof(Resources.Resource))]
        public required string FabricaId { get; set; }

        [Display(Name = "Fabrica", ResourceType = typeof(Resources.Resource))]
        [ValidateNever]
        public required Fabricas Fabrica { get; set; }


        /// <summary>
        /// Cliente
        /// </summary>
        [Display(Name = "Cliente", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessageResourceName = "CampoObrigatorio", ErrorMessageResourceType = typeof(Resources.Resource))]
        public required string ClienteId { get; set; }

        [Display(Name = "Cliente", ResourceType = typeof(Resources.Resource))]
        [ValidateNever]
        public required Clientes Cliente { get; set; }



        /// <summary>
        /// URL da imagem da peça
        /// </summary>
        [Display(Name = "ImagemPeca", ResourceType = typeof(Resources.Resource))]
        public string? Imagem { get; set; }

        public ICollection<LinhaEncomenda>? LinhasEncomenda { get; set; }
    }
}
