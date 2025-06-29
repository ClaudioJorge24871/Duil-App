﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Duil_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.ValueContentAnalysis;
using System.Runtime.Intrinsics.X86;
using Microsoft.Identity.Client;
using Duil_App.Resources;

namespace Duil_App.Controllers
{

    [Authorize(Roles = "Admin")]
    public class UtilizadoresController : Controller
    {

        /// <summary>
        /// Referência à Base de Dados do projeto
        /// </summary>
        private readonly Data.ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;


        public UtilizadoresController(UserManager<Utilizadores> userManager, Data.ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Utilizadores
        public async Task<IActionResult> Index(string texto, int? pageNumber)
        {

            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
            var adminIds = adminUsers.Select(u => u.Id).ToHashSet();

            var noAdmins = _context.Utilizadores.Where(u => !adminIds.Contains(u.Id)).AsQueryable();



            if (!String.IsNullOrEmpty(texto))
            {
                noAdmins = noAdmins.Where(t =>
                    t.Email!.ToUpper().Contains(texto.ToUpper()));
            }

            var pageSize = 10;
            return View(await PaginatedList<Utilizadores>.CreateAsync(noAdmins.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        // GET: Utilizadores/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            var role = await _userManager.GetRolesAsync(utilizador);
            switch (role[0])
            {
                case "Utilizador":
                case "User":
                    ViewBag.UserRole = Resource.UtilizadorRole;
                    break;

                case "Funcionario":
                case "Employee":
                    ViewBag.UserRole = Resource.FuncionarioRole;
                    break;

                case "Cliente":
                case "Client":
                    ViewBag.UserRole = Resource.ClienteRole;
                    break;

                default:
                    ViewBag.UserRole = role[0];
                    break;
            }
                

            return View(utilizador);
        }

        // GET: Utilizadores/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var utilizador = await _context.Utilizadores.FindAsync(id);
            if (utilizador == null)
                return NotFound();

            var userManager = HttpContext.RequestServices.GetRequiredService<UserManager<Utilizadores>>();
            var roles = await userManager.GetRolesAsync(utilizador);

            ViewBag.AllRoles = new List<string> { "Admin", Resource.ClienteRole, Resource.FuncionarioRole, Resource.UtilizadorRole};
            ViewBag.UserCurrentRole = roles.FirstOrDefault();

            return View(utilizador);
        }

        // POST: Utilizadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Nome,Morada,CodPostal,Pais,NIF,Telemovel")] Utilizadores utilizador, string SelectedRole)
        {
            if (id != utilizador.Id)
                return NotFound();

            //se o utilizador mudou o nome
            bool nomeEmUso = _context.Users.Any(user => user.Nome == utilizador.Nome && user.Id != id);

            if (nomeEmUso)
            {
                ModelState.AddModelError("Nome", "Já existe um utilizador com esse nome.");
                ViewBag.AllRoles = new List<string> { "Admin", "Cliente", "Funcionario", "Utilizador" };
                ViewBag.UserCurrentRole = SelectedRole;
                return View(utilizador);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var utilizadorBD = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Id == id);
                    if (utilizadorBD == null)
                        return NotFound();

                    utilizadorBD.Nome = utilizador.Nome;
                    utilizadorBD.Morada = utilizador.Morada;
                    utilizadorBD.CodPostal = utilizador.CodPostal?.ToUpper();
                    utilizadorBD.Pais = utilizador.Pais;
                    utilizadorBD.NIF = utilizador.NIF;
                    utilizadorBD.Telemovel = utilizador.Telemovel;

                    await _context.SaveChangesAsync();

                    var userManager = HttpContext.RequestServices.GetRequiredService<UserManager<Utilizadores>>();
                    var user = await userManager.FindByIdAsync(id);
                    var userRoles = await userManager.GetRolesAsync(user);

                    await userManager.RemoveFromRolesAsync(user, userRoles);
                    await userManager.AddToRoleAsync(user, SelectedRole);

                    // Se o novo papel for "Cliente", regista-o na tabela Clientes se ainda não existir
                    if (SelectedRole == "Cliente")
                    {
                        var clienteExistente = await _context.Clientes.FirstOrDefaultAsync(c => c.Nif == utilizador.NIF);
                        if (clienteExistente == null)
                        {
                            var novoCliente = new Clientes
                            {
                                Nome = utilizador.Nome,
                                Morada = utilizador.Morada,
                                CodPostal = utilizador.CodPostal,
                                Pais = utilizador.Pais,
                                Nif = utilizador.NIF,
                                Telemovel = utilizador.Telemovel,
                                Email = utilizador.Email,
                                MoradaCarga = utilizador.Morada == null ? string.Empty : utilizador.Morada,
                            };

                            _context.Clientes.Add(novoCliente);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilizadoresExists(utilizador.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }
            ViewBag.AllRoles = new List<string> { "Admin", "Cliente", "Funcionario", "Utilizador" };
            ViewBag.UserCurrentRole = SelectedRole;

            return View(utilizador);
        }


        private bool UtilizadoresExists(string id)
        {
            return _context.Utilizadores.Any(e => e.Id == id);
        }
    }
}
