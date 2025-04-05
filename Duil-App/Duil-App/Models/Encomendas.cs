using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Duil_App.Models
{
    /// <summary>
    /// Encomendas feitas pelos clientes
    /// </summary>
    public class Encomendas
    {
        /// <summary>
        /// Nosso ID da encomenda
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Id da encomenda do lado do cliente (empresa)
        /// </summary>
        public int Id_LadoCliente { get; set; }

        /// <summary>
        /// Fabrica fornecedora das pecas
        /// </summary>
        [ForeignKey(nameof(FabricaFornecedora))]
        public int FornecedoraFK { get; set; }
        public Fabricas FabricaFornecedora { get; set; }


        /// <summary>
        /// Identificacao da empresa compradora
        /// </summary>
        [ForeignKey(nameof(EmpresaCompradora))]
        public int CompradorFK {  get; set; }
        public Empresas EmpresaCompradora { get; set; }

        /// <summary>
        /// Total preco unitario da encomenda
        /// </summary>
        public decimal TotalPrecoUnit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int QuantidadeTotal { get; set; } 
    
        /// <summary>
        /// Transportadora do processo 
        /// </summary>
        public string? Transportadora { get; set; }

        /// <summary>
        /// Data em que a ecnomenda foi realizada
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Estado da encomenda
        /// </summary>
        public Estados Estado { get; set; }

        //Relacao N:N com Pecas
        public ICollection<EncomendasPecas> EncomendasPecas { get; set; }

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
