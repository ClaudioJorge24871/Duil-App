using System.ComponentModel.DataAnnotations;

namespace Duil_App.Models.ViewModels
{
    public class PecaDTO
    {
        public int Referencia { get; set; }

        public required string Designacao { get; set; } = string.Empty;

        //Questão aqui
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço unitário deve ser maior que 0.")]
        public decimal PrecoUnit { get; set; }

        public required string FabricaId { get; set; } 

        public required string ClienteId { get; set; } 

        public string? Imagem { get; set; } = null;
    }
}
