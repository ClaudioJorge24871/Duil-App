using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


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
        [Display(Name = "Referência")]
        [Required(ErrorMessage = "A referência é obrigatória.")]
        public int Referencia { get; set; }

        /// <summary>
        /// Designação da Peça. Simples nome ou descrição
        /// </summary>
        [StringLength(100)]
        [Display(Name = "Designação")]
        public string? Designacao { get; set; }

        /// <summary>
        /// Preco unitário
        /// </summary>
        [Display(Name = "Preço unitário")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço unitário deve ser maior que zero.")]
        public decimal PrecoUnit { get; set; }

        /// <summary>
        /// Fabricante
        /// </summary>
        [Required(ErrorMessage = "A identificação da fábrica é obrigatório")]
        [Display(Name = "Fábrica")]
        public string FabricaId { get; set; }

        [ValidateNever]
        public required Fabricas Fabrica { get; set; }

        public ICollection<LinhaEncomenda>? LinhasEncomenda { get; set; }
    }
}
