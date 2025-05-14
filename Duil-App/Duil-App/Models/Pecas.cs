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
        [Required(ErrorMessage = "A {0} é obrigatória.")]
        public int Referencia { get; set; }

        /// <summary>
        /// Designação da Peça. Simples nome ou descrição
        /// </summary>
        [StringLength(100)]
        [Display(Name = "Designação")]
        [Required(ErrorMessage = "A {0} é obrigatória.")]
        public required string Designacao { get; set; }

        /// <summary>
        /// Preco unitário
        /// </summary>
        [Display(Name = "Preço unitário")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O campo {0} tem de ser um número válido")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public decimal PrecoUnit { get; set; }

        /// <summary>
        /// Fabricante
        /// </summary>
        [Required(ErrorMessage = "A identificação da {0} é obrigatório")]
        [Display(Name = "Fábrica")]
        public required string FabricaId { get; set; }

        [ValidateNever]
        public required Fabricas Fabrica { get; set; }

        public ICollection<LinhaEncomenda>? LinhasEncomenda { get; set; }
    }
}
