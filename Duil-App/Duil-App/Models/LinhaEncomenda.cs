using System.ComponentModel.DataAnnotations;

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
        /// Identificador da encomenda
        /// </summary>
        public int EncomendaId { get; set; }
        public Encomendas Encomenda { get; set; }

        /// <summary>
        /// Identificador da peça 
        /// </summary>
        public int PecaId { get; set; }
        public Pecas Peca { get; set; }

        /// <summary>
        /// Quantidade de unidades da peça na encomenda
        /// </summary>
        public int Quantidade { get; set; }
    }
}
