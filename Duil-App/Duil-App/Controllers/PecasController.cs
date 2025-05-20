
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
using Microsoft.Extensions.Hosting.Internal;

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
            var pecas = await _context.Pecas
                .Include(p => p.Fabrica)
                .Include(p => p.Cliente)
                .ToListAsync();

            return View(pecas);
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
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pecas == null)
            {
                return NotFound();
            }

            ViewData["FabricaNome"] = pecas.Fabrica?.Nome;
            ViewData["ClienteNome"] = pecas.Cliente?.Nome;

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
        public async Task<IActionResult> Create([Bind("Referencia,Designacao,PrecoUnit,FabricaId,ClienteId")] Pecas pecas,
            IFormFile imagemFile, 
            [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            if (ModelState.IsValid)
            {
                if (imagemFile != null && imagemFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    var fileExtension = Path.GetExtension(imagemFile.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("Imagem", "Apenas imagens nos formatos JPEG, PNG, GIF ou WEBP são permitidas.");
                        return View(pecas);
                    }


                    try
                    {
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + imagemFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        Directory.CreateDirectory(uploadsFolder); 

                        using(var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imagemFile.CopyToAsync(fileStream);
                        }
                        pecas.Imagem = uniqueFileName;
                    }catch (Exception ex) {
                        ModelState.AddModelError("Imagem", "Erro ao fazer upload da imagem: " + ex.Message);
                        return View(pecas);
                    }
                }


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

            var peca = await _context.Pecas
                .Include(p => p.Fabrica)
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (peca == null)
            {
                return NotFound();
            }
            var NomeFabrica = _context.Fabricas
                .Where(f => f.Nif == peca.FabricaId)
                .Select(f => f.Nome)
                .FirstOrDefault();

            var NomeCliente= _context.Clientes
                .Where(f => f.Nif == peca.ClienteId)
                .Select(f => f.Nome)
                .FirstOrDefault();

            ViewData["FabricaNome"] = peca.Fabrica?.Nome;
            ViewData["FabricaId"] = peca.FabricaId;

            ViewData["ClienteNome"] = peca.Cliente?.Nome;
            ViewData["ClienteId"] = peca.ClienteId;

            return View(peca);
        }

        // POST: Pecas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
    int id,
    [Bind("Id,Referencia,Designacao,PrecoUnit,FabricaId,ClienteId")] Pecas peca,
    IFormFile imagemFile,
    [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            if (id != peca.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPeca = await _context.Pecas.FindAsync(peca.Id);
                    if (existingPeca == null)
                    {
                        return NotFound();
                    }

                    if (_context.Pecas.Any(p => p.Referencia == peca.Referencia && p.Id != peca.Id))
                    {
                        ModelState.AddModelError("Referencia", "Esta referência já existe.");
                        return View(peca);
                    }

                    if (imagemFile != null && imagemFile.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                        var fileExtension = Path.GetExtension(imagemFile.FileName).ToLowerInvariant();
                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("Imagem", "Apenas imagens nos formatos JPEG, PNG, GIF ou WEBP são permitidas.");
                            return View(peca);
                        }

                        if (!string.IsNullOrEmpty(existingPeca.Imagem))
                        {
                            string oldFilePath = Path.Combine(hostingEnvironment.WebRootPath, "images", existingPeca.Imagem);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + imagemFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        Directory.CreateDirectory(uploadsFolder);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imagemFile.CopyToAsync(fileStream);
                        }

                        existingPeca.Imagem = uniqueFileName;
                    }

                    existingPeca.Referencia = peca.Referencia;
                    existingPeca.Designacao = peca.Designacao;
                    existingPeca.PrecoUnit = peca.PrecoUnit;
                    existingPeca.FabricaId = peca.FabricaId;
                    existingPeca.ClienteId = peca.ClienteId;

                    _context.Update(existingPeca);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PecasExists(peca.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(peca);
        }


        // GET: Pecas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peca = await _context.Pecas
                .Include(p => p.Fabrica)
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (peca == null)
            {
                return NotFound();
            }
            var NomeFabrica = _context.Fabricas
                .Where(f => f.Nif == peca.FabricaId)
                .Select(f => f.Nome)
                .FirstOrDefault();

            var NomeCliente = _context.Clientes
                .Where(f => f.Nif == peca.ClienteId)
                .Select(f => f.Nome)
                .FirstOrDefault();

            ViewData["FabricaNome"] = peca.Fabrica?.Nome;
            ViewData["ClienteNome"] = peca.Cliente?.Nome;

            return View(peca);
        }

        // POST: Pecas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var peca = await _context.Pecas.FindAsync(id);
            if (peca != null)
            {
                _context.Pecas.Remove(peca);
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


