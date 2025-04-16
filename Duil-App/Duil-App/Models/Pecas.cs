using System.ComponentModel.DataAnnotations;

namespace Duil_App.Models
{
    /// <summary>
    /// Pecas que fazem parte de uma encomenda. 
    /// Possui referência propria, preço, designação e identificaçaõ da fábrica que produz
    /// </summary>
    public class Pecas
    {
        /// <summary>
        /// Referência da peça
        /// </summary>
        [Key]
        public int Referencia { get; set; }
        
        /// <summary>
        /// Designação da Peça. Simples nome ou descrição
        /// </summary>
        public string Designacao { get; set; }

        /// <summary>
        /// Preco unitário
        /// </summary>
        public decimal PrecoUnit { get; set; }



        public int FabricaId { get; set; }

        public Fabricas Fabrica { get; set; }

        public ICollection<LinhaEncomenda> LinhasEncomenda { get; set; }
    }
}
