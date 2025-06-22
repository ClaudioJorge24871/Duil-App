namespace Duil_App.Models.ViewModels
{
    public class PecaDTO
    {
        public int Referencia { get; set; }

        public string Designacao { get; set; } = string.Empty;

        public decimal PrecoUnit { get; set; }

        public string FabricaId { get; set; } = string.Empty;

        public string ClienteId { get; set; } = string.Empty;

        public string? Imagem { get; set; } = null;
    }
}
