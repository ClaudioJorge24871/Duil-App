
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Duil_App.Data;
using Duil_App.Models;
using Microsoft.AspNetCore.Authorization;
using Duil_App.Code;

namespace Duil_App.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Método auxiliar para obter lista de páises 
        private List<SelectListItem> ObterPaises()
        {
            return ListasHelper.ObterListaDePaises();
        }

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listagem de clientes apenas acessível para Admin e Funcionário
        [Authorize(Roles = "Admin, Funcionario")]
        public async Task<IActionResult> Index(string texto, int? pageNumber)
        {
            if (_context.Clientes == null)
            {
                return Problem("Clientes é um valor null.");
            }

            // Aplicar filtro se houver texto de pesquisa
            var clientes = from c in _context.Clientes
                           select c;

            if (!String.IsNullOrEmpty(texto))
            {
                clientes = clientes.Where(s => s.Nome!.ToUpper().Contains(texto.ToUpper()));
            }

            // Paginação com 10 resultados
            int pageSize = 10;
            return View(await PaginatedList<Clientes>.CreateAsync(clientes.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        // Retorna sugestões de nomes de clientes para autocomplete
        [HttpGet]
        public IActionResult Search(string term)
        {
            var resultados = _context.Clientes
                .Where(c => c.Nome.Contains(term))
                .Select(c => new
                {
                    label = c.Nome,
                    value = c.Nif
                })
                .Take(10)
                .ToList();

            return Json(resultados);
        }

        // Detalhes de um cliente 
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Nif == id);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewBag.Pais = ObterPaises();
            return View(cliente);
        }

        // Criação de cliente
        [Authorize(Roles = "Admin")]
  
        public IActionResult Create()
        {
            ViewBag.Pais = ObterPaises();
            return View();
        }

        [Authorize(Roles = "Admin")]

        // Submissão do formulário de criação
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MoradaCarga,Nif,Nome,Morada,CodPostal,Pais,Telemovel,Email")] Clientes cliente)
        {
            // Validação de nif
            if (_context.Clientes.Any(c => c.Nif == cliente.Nif))
            {
                ModelState.AddModelError("Nif", "Já existe um cliente com este NIF.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(cliente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao guardar os dados. Nif inserido já existe");
                }
            }

            ViewBag.Pais = ObterPaises();
            return View(cliente);

        }

        // Formulário de edição de um cliente
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewBag.Pais = ObterPaises();
            return View(cliente);
        }

        // Submissão do formulario de edição
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MoradaCarga,Nif,Nome,Morada,CodPostal,Pais,Telemovel,Email")] Clientes cliente)
        {
            ViewBag.Pais = ObterPaises();
            if (id != cliente.Nif)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientesExists(cliente.Nif))
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

            ViewBag.Pais = ObterPaises();
            return View(cliente);
        }

        // Confirmação da eliminação de um cliente
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Nif == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientesExists(string id)
        {
            return _context.Clientes.Any(e => e.Nif == id);
        }


    }


}
