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
    public class EncomendasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EncomendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Encomendas
                .Include(e => e.Cliente)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var encomendas = await _context.Encomendas
                .Include(e => e.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);

            return encomendas == null ? NotFound() : View(encomendas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    [Bind("Id,IdLadoCliente,Data,Transportadora,Estado,ClienteId,TotalPrecoUnit,QuantidadeTotal")] Encomendas encomenda,
    List<int> pecasSelecionadas,
    List<int> quantidades)
        {
            encomenda.Estado = Estados.Pendente;

            if (pecasSelecionadas == null || quantidades == null || !pecasSelecionadas.Any())
            {
                ModelState.AddModelError("", "Selecione pelo menos uma peça");
            }
            else
            {
                try
                {
                    var pecas = await _context.Pecas
                        .Where(p => pecasSelecionadas.Contains(p.Id))
                        .ToListAsync();

                    // Manual decimal parsing to handle culture differences
                    encomenda.QuantidadeTotal = quantidades.Sum();
                    encomenda.TotalPrecoUnit = pecas
                        .Zip(quantidades, (p, q) => p.PrecoUnit * q)
                        .Sum();
                }
                catch
                {
                    ModelState.AddModelError("", "Erro no cálculo dos valores");
                }
            }

            // Server-side validation
            if (encomenda.TotalPrecoUnit < 0.01m)
            {
                ModelState.AddModelError("TotalPrecoUnit", "O valor total deve ser maior que zero");
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(encomenda);
                    await _context.SaveChangesAsync();

                    if (pecasSelecionadas != null && quantidades != null)
                    {
                        for (int i = 0; i < pecasSelecionadas.Count; i++)
                        {
                            var linha = new LinhaEncomenda
                            {
                                EncomendaId = encomenda.Id,
                                PecaId = pecasSelecionadas[i],
                                Quantidade = quantidades[i]
                            };
                            _context.LinhasEncomendas.Add(linha);
                        }
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }
            else
            {
                // Add error logging for debugging
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
            }

            return View(encomenda);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var encomenda = await _context.Encomendas
                .Include(e => e.Cliente)
                .Include(e => e.LinhasEncomenda)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (encomenda == null) return NotFound();

            ViewData["Estado"] = new SelectList(Enum.GetValues(typeof(Estados)), encomenda.Estado);
            return View(encomenda);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
    [Bind("Id,IdLadoCliente,Data,Transportadora,Estado,ClienteId,TotalPrecoUnit,QuantidadeTotal")] Encomendas encomenda,
    List<int> pecasSelecionadas,
    List<int> quantidades)
        {
            if (id != encomenda.Id) return NotFound();

            if (pecasSelecionadas != null && quantidades != null)
            {
                // Recalculate totals
                var pecas = await _context.Pecas
                    .Where(p => pecasSelecionadas.Contains(p.Id))
                    .ToListAsync();

                encomenda.QuantidadeTotal = quantidades.Sum();
                encomenda.TotalPrecoUnit = pecas
                    .Zip(quantidades, (p, q) => p.PrecoUnit * q)
                    .Sum();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(encomenda);

                    var existingLines = await _context.LinhasEncomendas
                        .Where(le => le.EncomendaId == id)
                        .ToListAsync();
                    _context.LinhasEncomendas.RemoveRange(existingLines);

                    if (pecasSelecionadas != null && quantidades != null)
                    {
                        for (int i = 0; i < pecasSelecionadas.Count; i++)
                        {
                            var linha = new LinhaEncomenda
                            {
                                EncomendaId = encomenda.Id,
                                PecaId = pecasSelecionadas[i],
                                Quantidade = quantidades[i]
                            };
                            _context.LinhasEncomendas.Add(linha);
                        }
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!EncomendasExists(encomenda.Id)) throw;
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }
            return View(encomenda);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var encomenda = await _context.Encomendas
                .FirstOrDefaultAsync(m => m.Id == id);

            return encomenda == null ? NotFound() : View(encomenda);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var encomenda = await _context.Encomendas.FindAsync(id);
            if (encomenda != null)
            {
                _context.Encomendas.Remove(encomenda);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EncomendasExists(int id)
        {
            return _context.Encomendas.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> GetPecasPorCliente(string clienteId)
        {
            var pecas = await _context.Pecas
                .Where(p => p.ClienteId == clienteId)
                .Select(p => new {
                    id = p.Id,
                    nome = p.Designacao,
                    preco = p.PrecoUnit
                })
                .ToListAsync();

            return Json(pecas);
        }
    }
}