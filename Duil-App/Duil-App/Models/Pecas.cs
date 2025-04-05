namespace Duil_App.Models
{
    /// <summary>
    /// Pecas
    /// </summary>
    public class Pecas
    {
        /// <summary>
        /// Identificacao da peca
        /// </summary>
        public int Referencia { get; set; }

        /// <summary>
        /// Designacao da peca
        /// </summary>
        public string? Designacao { get; set; }

        /// <summary>
        /// Preco unitario da peca
        /// </summary>
        public decimal PrecoUnit { get; set; }

        public ICollection<EncomendasPecas> EncomendasPeca { get; set; }
    }
}
