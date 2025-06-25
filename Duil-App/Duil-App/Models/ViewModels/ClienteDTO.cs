namespace Duil_App.Models.ViewModels
{
    public class ClienteDTO
    {
        public required string Nif { get; set; }

        public required string Nome { get; set; }

        public string? Morada { get; set; }

        public string? CodPostal { get; set; }

        public required string Pais { get; set; }

        public string? Telemovel { get; set; }

        public string? Email { get; set; }

        public string? MoradaCarga { get; set; }
        

    }
}
