using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Duil_App.Data;
using Duil_App.Models;
using Microsoft.AspNetCore.Authorization;

namespace Duil_App.Controllers
{
    // Apenas acessível a Admin e Funcionário
    [Authorize(Roles = "Admin,Funcionario")]
    public class FabricasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FabricasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listagem de fábricas
        public async Task<IActionResult> Index(string texto, int? pageNumber)
        {
            if (_context.Fabricas == null)
            {
                return Problem("Fabricas é um valor null.");
            }

            var fabricas = from c in _context.Fabricas
                           select c;

            // Aplicar filtro se houver texto de pesquisa
            if (!String.IsNullOrEmpty(texto))
            {
                fabricas = fabricas.Where(s => s.Nome!.ToUpper().Contains(texto.ToUpper()));
            }

            // Paginação com 10 resultados

            var pageSize = 10;
            return View(await PaginatedList<Fabricas>.CreateAsync(fabricas.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        // Retorna sugestões de nomes de fábricas para autocomplete
        [HttpGet]
        public IActionResult Search(string term)
        {
            var resultados = _context.Fabricas
                .Where(f => f.Nome.Contains(term))
                .Select(f => new {
                    label = f.Nome,
                    value = f.Nif
                })
                .Take(10)
                .ToList();

            return Json(resultados);
        }

        // Detalhes de um cliente especifico
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

        // Criação de fábricas
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
            // Validação de nif
            if (_context.Clientes.Any(c => c.Nif == fabrica.Nif))
            {
                ModelState.AddModelError("Nif", "Já existe uma fábrica ou cliente com este NIF.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(fabrica);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao guardar os dados. Nif inserido já existe");
                }
            }

            return View(fabrica);
        }

        // Formulário de edição de uma fábrica
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

        // Submissão do formulario de edição
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

        // Confirmação da eliminação de uma fábrica
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

        // Eliminação efetiva de uma fábrica
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
