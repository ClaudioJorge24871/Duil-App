using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Duil_App.Data;
using Duil_App.Models;
using System.Configuration;

namespace Duil_App.Controllers
{
    public class PecasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PecasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pecas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pecas.ToListAsync());
        }

        // GET: Pecas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pecas = await _context.Pecas
                .Include(p => p.Fabrica)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pecas == null)
            {
                return NotFound();
            }

            ViewData["FabricaNome"] = pecas.Fabrica?.Nome; 

            return View(pecas);
        }

        // GET: Pecas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pecas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Referencia,Designacao,PrecoUnit,FabricaId")] Pecas pecas)
        {
            if (ModelState.IsValid)
            {
               
                if (_context.Pecas.Any(p => p.Referencia == pecas.Referencia))
                {
                    ModelState.AddModelError("Referencia", "Esta referência já existe.");
                    return View(pecas);
                }

                _context.Add(pecas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pecas);
        }

        // GET: Pecas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pecas = await _context.Pecas
                .Include(p => p.Fabrica) 
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pecas == null)
            {
                return NotFound();
            }
            var NomeFabrica = _context.Fabricas
                .Where(f => f.Nif == pecas.FabricaId)
                .Select(f => f.Nome)
                .FirstOrDefault();

            ViewData["FabricaNome"] = pecas.Fabrica?.Nome;
            ViewData["FabricaId"] = pecas.FabricaId;

            return View(pecas);
        }

        // POST: Pecas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Referencia,Designacao,PrecoUnit,FabricaId")] Pecas pecas)
        {
            if (id != pecas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pecas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PecasExists(pecas.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pecas);
        }


        // GET: Pecas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pecas = await _context.Pecas
                .Include(p => p.Fabrica)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pecas == null)
            {
                return NotFound();
            }
            var NomeFabrica = _context.Fabricas
                .Where(f => f.Nif == pecas.FabricaId)
                .Select(f => f.Nome)
                .FirstOrDefault();

            ViewData["FabricaNome"] = pecas.Fabrica?.Nome;

            return View(pecas);
        }

        // POST: Pecas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pecas = await _context.Pecas.FindAsync(id);
            if (pecas != null)
            {
                _context.Pecas.Remove(pecas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PecasExists(int id)
        {
            return _context.Pecas.Any(e => e.Id == id);
        }



    }
}
