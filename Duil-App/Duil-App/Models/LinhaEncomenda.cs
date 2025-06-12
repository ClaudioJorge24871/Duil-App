using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Duil_App.Models
{
    /// <summary>
    /// Classe de relação das encomendas e peças
    /// </summary>
    public class LinhaEncomenda
    {
        /// <summary>
        /// Identificador da peça encomenda
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Identificador da encomenda FK
        /// </summary>
        [Required]
        [ForeignKey("Encomenda")]
        public int EncomendaId { get; set; }
        
        
        /// <summary>
        /// Relacionamento para encomendas
        /// </summary>
        public Encomendas? Encomenda { get; set; }

        /// <summary>
        /// Identificador da peça 
        /// </summary>
        [Required]
        [ForeignKey("Peca")]
        public int PecaId { get; set; }
        public Pecas? Peca { get; set; }

        /// <summary>
        /// Quantidade de unidades da peça na encomenda
        /// </summary>
        public int Quantidade { get; set; }
    }
}
