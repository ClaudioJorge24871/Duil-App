namespace Duil_App.Models.ViewModels
{
    public class UtilizadorDTO
    {
        public required string Nome { get; set; } = string.Empty;

        public string? Morada { get; set; }

        public string? CodPostal { get; set; }

        public required string Pais { get; set; } 

        public required string NIF { get; set; } = string.Empty;

        public string? Telemovel { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
