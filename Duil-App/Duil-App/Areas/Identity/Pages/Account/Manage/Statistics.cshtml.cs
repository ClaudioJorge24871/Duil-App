using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Duil_App.Data;
using Duil_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Duil_App.Models.ViewModels;

namespace Duil_App.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// Modelo da página para apresentar estatísticas do utilizador 
    /// </summary>
    public class StatisticsModel : PageModel
    {
        private readonly UserManager<Utilizadores> _userManager;
        private readonly ApplicationDbContext _context;

        public StatisticsModel(UserManager<Utilizadores> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public int TotalEncomendas  { get; set; }
        public int EncomendasConcretizadas { get; set; }
        public int NumeroPecas { get; set; }

        public DateTime? DataUltimaEncomenda { get; set; }

        public DateTime? DataPrimeiraEncomenda { get; set; }
        public string? PecaMaisEncomendada { get; set; }
        public int TotalQuantidadePecas { get; set; }


        // Estados:
        public int NumPendente { get; set; }
        public int NumConfirmada { get; set; }
        public int NumConcretizada { get; set; }
        public int NumCancelada { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("Utilizador não encontrado");

            TotalEncomendas = await _context.Encomendas.CountAsync(e => e.ClienteId == user.NIF);

            EncomendasConcretizadas = await _context.Encomendas.CountAsync(e=> e.ClienteId == user.NIF && e.Estado == Estados.Concretizada);

            NumeroPecas = await _context.Pecas.CountAsync(p => p.ClienteId == user.NIF);

            DataUltimaEncomenda = await _context.Encomendas.Where(e => e.ClienteId == user.NIF)
                .OrderByDescending(e => e.Data)
                .Select(e => e.Data)
                .FirstOrDefaultAsync();

            DataPrimeiraEncomenda = await _context.Encomendas.Where(e => e.ClienteId == user.NIF)
                .OrderBy(e => e.Data)
                .Select(e => e.Data)
                .FirstOrDefaultAsync();
            
            // toal de peças encomendadas
            TotalQuantidadePecas = await _context.LinhasEncomendas.Where(le => le.Encomenda.ClienteId == user.NIF)
                .SumAsync(le => (int?)le.Quantidade) ?? 0;


            PecaMaisEncomendada = await _context.LinhasEncomendas.Where(le => le.Encomenda.ClienteId == user.NIF)
                .GroupBy(le => le.Peca.Designacao)
                .OrderByDescending(p => p.Sum(le => le.Quantidade))
                .Select(p => p.Key)
                .FirstOrDefaultAsync();

            NumPendente = await _context.Encomendas.CountAsync(e => e.ClienteId == user.NIF && e.Estado == Estados.Pendente);
            NumConfirmada = await _context.Encomendas.CountAsync(e => e.ClienteId == user.NIF && e.Estado == Estados.Confirmada);
            NumConcretizada = await _context.Encomendas.CountAsync(e => e.ClienteId == user.NIF && e.Estado == Estados.Concretizada);
            NumCancelada = await _context.Encomendas.CountAsync(e => e.ClienteId == user.NIF && e.Estado == Estados.Cancelada);


            return Page();
        }
    }
}
