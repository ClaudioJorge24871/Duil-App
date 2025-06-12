using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Duil_App.Models;

namespace Duil_App.Controllers {
   public class UtilizadoresController: Controller {

      /// <summary>
      /// Referência à Base de Dados do projeto
      /// </summary>
      private readonly Data.ApplicationDbContext _context;

      public UtilizadoresController(Data.ApplicationDbContext context) {
         _context = context;
      }

      // GET: Utilizadores
      public async Task<IActionResult> Index() {

         // procurar na BD todos os utilizadores e listá-los
         // entregando, de seguida, esses dados à View
         // SELECT *
         // FROM Utilizadores
         return View(await _context.Utilizadores.ToListAsync());
      }

      // GET: Utilizadores/Details/5
      public async Task<IActionResult> Details(string? id) {
         if (id == null) {
            return NotFound();
         }

         var utilizador = await _context.Utilizadores
             .FirstOrDefaultAsync(m => m.Id == id);
         if (utilizador == null) {
            return NotFound();
         }

         return View(utilizador);
      }

      // GET: Utilizadores/Create
      public IActionResult Create() {
         return View();
      }

      // POST: Utilizadores/Create
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create([Bind("Nome,Morada,CodPostal,Pais,NIF,Telemovel")] Utilizadores utilizador) {

         if (ModelState.IsValid) {
            // Corrigir os dados do Código Postal
            utilizador.CodPostal = utilizador.CodPostal?.ToUpper();

            _context.Add(utilizador);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
         }
         return View(utilizador);
      }

      // GET: Utilizadores/Edit/5
      public async Task<IActionResult> Edit(int? id) {
         if (id == null) {
            return NotFound();
         }

         var utilizador = await _context.Utilizadores.FindAsync(id);
         if (utilizador == null) {
            return NotFound();
         }
         return View(utilizador);
      }

      // POST: Utilizadores/Edit/5
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(string id, [Bind("Id,Nome,Morada,CodPostal,Pais,NIF,Telemovel")] Utilizadores utilizador) {
         if (id != utilizador.Id) {
            return NotFound();
         }

         if (ModelState.IsValid) {
            try {
               // Corrigir os dados do Código Postal
               utilizador.CodPostal = utilizador.CodPostal?.ToUpper();

               _context.Update(utilizador);
               await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
               if (!UtilizadoresExists(utilizador.Id)) {
                  return NotFound();
               }
               else {
                  throw;
               }
            }
            return RedirectToAction(nameof(Index));
         }
         return View(utilizador);
      }

      // GET: Utilizadores/Delete/5
      public async Task<IActionResult> Delete(string? id) {
         if (id == null) {
            return NotFound();
         }

         var utilizador = await _context.Utilizadores
             .FirstOrDefaultAsync(m => m.Id == id);
         if (utilizador == null) {
            return NotFound();
         }

         return View(utilizador);
      }

      // POST: Utilizadores/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id) {
         var utilizador = await _context.Utilizadores.FindAsync(id);
         if (utilizador != null) {
            _context.Utilizadores.Remove(utilizador);
         }

         await _context.SaveChangesAsync();
         return RedirectToAction(nameof(Index));
      }

      private bool UtilizadoresExists(string id) {
         return _context.Utilizadores.Any(e => e.Id == id);
      }
   }
}
