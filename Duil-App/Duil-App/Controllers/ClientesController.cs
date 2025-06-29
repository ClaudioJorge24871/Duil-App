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
using Duil_App.Code;
using Duil_App;

namespace Duil_App.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private List<SelectListItem> ObterPaises()
        {
            return ListasHelper.ObterListaDePaises();
        }

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        [Authorize(Roles = "Admin, Funcionario")]
        public async Task<IActionResult> Index(string texto, int? pageNumber)
        {
            if (_context.Clientes == null)
            {
                return Problem("Clientes é um valor null.");
            }

            var clientes = from c in _context.Clientes
                           select c;

            if (!String.IsNullOrEmpty(texto))
            {
                clientes = clientes.Where(s => s.Nome!.ToUpper().Contains(texto.ToUpper()));
            }

            int pageSize = 10;
            return View(await PaginatedList<Clientes>.CreateAsync(clientes.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        // GET: Clientes
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

        // GET: Clientes/Details/5
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

        [Authorize(Roles = "Admin")]
        // GET: Clientes/Create
        public IActionResult Create()
        {
            ViewBag.Pais = ObterPaises();
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MoradaCarga,Nif,Nome,Morada,CodPostal,Pais,Telemovel,Email")] Clientes cliente)
        {
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

        [Authorize(Roles = "Admin")]
        // GET: Clientes/Edit/5
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

        [Authorize(Roles = "Admin")]
        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        [Authorize(Roles = "Admin")]
        // GET: Clientes/Delete/5
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
        // POST: Clientes/Delete/5
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
