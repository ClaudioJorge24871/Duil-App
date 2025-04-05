namespace Duil_App.Models
{
    /// <summary>
    /// Tabela de relacionamento das pecas e encomendas
    /// </summary>
    public class EncomendasPecas
    {
        /// <summary>
        /// Identificacao da encomenda
        /// </summary>
        public int EncomendaId { get; set; }
        public Encomendas Encomenda {  get; set; }

        /// <summary>
        /// Identificacao da peca
        /// </summary>
        public int PecaId { get; set; }
        public Pecas Peca { get; set; }

        /// <summary>
        /// Quantidade de pecas na encomend
        /// </summary>
        public int QuantidadePecas { get; set; }
    }
}
