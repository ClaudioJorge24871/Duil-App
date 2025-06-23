namespace Duil_App.Models.ViewModels
{
    public class EncomendaDTO
    {
        /// <summary>
        /// Identificador da encomenda do lado do cliente (obrigatório)
        /// </summary>
        public int IdLadoCliente { get; set; }

        public string? Transportadora { get; set; } = string.Empty;

        public required string ClienteId { get; set; }

    }
}
