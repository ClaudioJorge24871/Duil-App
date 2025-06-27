using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Duil_App.Data;
using Duil_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("Utilizador não encontrado");

            TotalEncomendas = await _context.Encomendas.CountAsync(e => e.ClienteId == user.NIF);

            EncomendasConcretizadas = await _context.Encomendas.CountAsync(e=> e.ClienteId == user.NIF && e.Estado == Estados.Concretizada);

            NumeroPecas = await _context.Pecas.CountAsync(p => p.ClienteId == user.NIF);

            return Page();
        }
    }
}
