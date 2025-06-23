namespace Duil_App.Models.ViewModels
{
    public class EncomendaDTO
    {
        public int IdLadoCliente { get; set; }

        public string? Transportadora { get; set; } = string.Empty;

        public required string ClienteId { get; set; }

    }
}
