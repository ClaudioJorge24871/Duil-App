using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Duil_App.Data;
using Duil_App.Models;

namespace Duil_App.Controllers
{
    public class FabricasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FabricasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Fabricas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fabricas.ToListAsync());
        }

        // GET: Fabricas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabrica = await _context.Fabricas
                .FirstOrDefaultAsync(m => m.Nif == id);
            if (fabrica == null)
            {
                return NotFound();
            }

            return View(fabrica);
        }

        // GET: Fabricas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fabricas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MoradaDescarga,Nif,Nome,Morada,CodPostal,Pais,Telemovel,Email")] Fabricas fabrica)
        {

            if (_context.Clientes.Any(c => c.Nif == fabrica.Nif))
            {
                ModelState.AddModelError("Nif", "Já existe uma fábrica com este NIF.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(fabrica);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao guardar os dados. Nif inserido já existe");
                }
            }

            return View(fabrica);
        }

        // GET: Fabricas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabrica = await _context.Fabricas.FindAsync(id);
            if (fabrica == null)
            {
                return NotFound();
            }
            return View(fabrica);
        }

        // POST: Fabricas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MoradaDescarga,Nif,Nome,Morada,CodPostal,Pais,Telemovel,Email")] Fabricas fabrica)
        {
            if (id != fabrica.Nif)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fabrica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FabricasExists(fabrica.Nif))
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
            return View(fabrica);
        }

        // GET: Fabricas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabrica = await _context.Fabricas
                .FirstOrDefaultAsync(m => m.Nif == id);
            if (fabrica == null)
            {
                return NotFound();
            }

            return View(fabrica);
        }

        // POST: Fabricas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var fabrica = await _context.Fabricas.FindAsync(id);
            if (fabrica != null)
            {
                _context.Fabricas.Remove(fabrica);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FabricasExists(string id)
        {
            return _context.Fabricas.Any(e => e.Nif == id);
        }
    }
}
