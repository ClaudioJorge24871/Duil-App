using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Duil_App.Data;
using Duil_App.Models;
using Microsoft.AspNetCore.Authorization;


namespace Duil_App.Controllers
{
    // Acessível apenas para Admin e Funcionario
    [Authorize(Roles = "Admin,Funcionario")]
    public class PecasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PecasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listagem de peças
        public async Task<IActionResult> Index(string texto, int? pageNumber)
        {
            var pecas = _context.Pecas
                .Include(p => p.Fabrica)
                .Include(p => p.Cliente)
                .AsQueryable();

            // Aplicar filtro se houver texto de pesquisa
            if (!string.IsNullOrEmpty(texto))
            {
                pecas = pecas.Where(s => s.Designacao.ToUpper().Contains(texto.ToUpper()));
            }

            // Paginação com 10 resultados
            int pageSize = 10;
            return View(await PaginatedList<Pecas>.CreateAsync(pecas.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // Detalhes de uma peça
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

        //  Criação de peça
        public IActionResult Create()
        {
            return View();
        }

        // Submissão do formulário de criação
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Referencia,Designacao,PrecoUnit,FabricaId,ClienteId")] Pecas pecas,
            IFormFile? imagemFile, 
            [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            if (ModelState.IsValid)
            {   
                // Colocar imagem default se nenhuma for fornecida
                if (imagemFile != null && imagemFile.Length > 0)
                {
                    // Validação deoficheiro imagem
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

                    } catch (Exception ex) {
                        ModelState.AddModelError("Imagem", "Erro ao fazer upload da imagem: " + ex.Message);
                        return View(pecas);
                    }
                }
                else
                {
                    // Define a imagem padrão
                    pecas.Imagem = "Default_Image.jpg";
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

        // Formulario de edição de uma peça
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

       // sSubmissão do formulário de uma peça
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
    int id,
    [Bind("Id,Referencia,Designacao,PrecoUnit,FabricaId,ClienteId,Imagem")] Pecas peca,
    IFormFile imagemFile,
    [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            if (id != peca.Id)
            {
                return NotFound();
            }

            if (imagemFile != null && imagemFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(imagemFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("Imagem", "Apenas imagens nos formatos JPEG, PNG, GIF ou WEBP são permitidas.");
                }
            }
            else
            {
                ModelState.Remove(nameof(imagemFile));
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

                    string currentImage = existingPeca.Imagem;

                    if (_context.Pecas.Any(p => p.Referencia == peca.Referencia && p.Id != peca.Id))
                    {
                        ModelState.AddModelError("Referencia", "Esta referência já existe.");
                        return View(peca);
                    }

                    if (imagemFile != null && imagemFile.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(currentImage))
                        {
                            string oldFilePath = Path.Combine(hostingEnvironment.WebRootPath, "images", currentImage);
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
                    else
                    {
                        existingPeca.Imagem = currentImage;
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


        // Confirmação da elimnação de uma peça
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

        // Eliminação de uma peça
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var peca = await _context.Pecas.FindAsync(id);
            if (peca != null)
            {
                // Se a Peça tiver encomendas associadas, não apaga a peça
                bool temEncomendas = await _context.LinhasEncomendas
            .AnyAsync(le => le.PecaId == id);

                if (temEncomendas)
                {
                    // Mostra uma mensagem de erro
                    TempData["ErrorMessage"] = "Não foi possível remover esta peça. Motivo: Existem encomendas associadas à mesma.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Pecas.Remove(peca);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PecasExists(int id)
        {
            return _context.Pecas.Any(e => e.Id == id);
        }


    }
}


