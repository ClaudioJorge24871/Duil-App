using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Duil_App.Data;
using Duil_App.Models;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Duil_App.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting.Internal;
using Duil_App.Code;
using Duil_App.Resources;

namespace Duil_App.Controllers
{

    public class EncomendasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;
        private readonly IHubContext<RealTimeHub> _hubcontext;

        public EncomendasController(
                ApplicationDbContext context,
                UserManager<Utilizadores> userManager,
                IHubContext<RealTimeHub> hubContext
            )
        {
            _context = context;
            _userManager = userManager;
            _hubcontext = hubContext;
        }

        // Listagem de encomendas
        public async Task<IActionResult> Index(String texto, int? pageNumber)
        {
            if (User.IsInRole("Cliente"))
            {
                await GetClientesEncomendas();
            }

            if (_context.Encomendas == null)
            {
                return Problem("Encomendas é um valor null.");
            }

            // Incluir informação dos clientes
            var encomendas = _context.Encomendas
              .Include(e => e.Cliente)
              .AsQueryable();

            // Aplicar filtro se houver texto de pesquisa
            if (!String.IsNullOrEmpty(texto))
            {
                encomendas = encomendas.Where(t =>
                    t.Cliente.Nome.ToUpper().Contains(texto.ToUpper()));
            }

            // Paginação com 10 resultados
            var pageSize = 10;
            return View(await PaginatedList<Encomendas>.CreateAsync(encomendas.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // Detalhes de uma encomenda especifico
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var encomendas = await _context.Encomendas
                .Include(e => e.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);

			ViewBag.estadoEncomenda = EstadosTraduzidosHelper.GetUmEstadoTraduzido(encomendas.Estado.ToString());

			return encomendas == null ? NotFound() : View(encomendas);
        }

        /// <summary>
        /// GET CREATE: Encomendas
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            if (User.IsInRole("Cliente"))
            {
                var userId = _userManager.GetUserId(User);
                var user = _userManager.GetUserAsync(User).Result;
                var userIdentificador = user.NIF;

                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                ViewBag.ClienteId = userIdentificador;
            }

            return View();
        }

        /// <summary>
        /// Post CREATE:  Encomendas
        /// </summary>
        /// <param name="encomenda"></param>
        /// <param name="pecasSelecionadas"></param>
        /// <param name="quantidades"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,IdLadoCliente,Data,Transportadora,Estado,ClienteId,TotalPrecoUnit,QuantidadeTotal")] Encomendas encomenda,
            List<int> pecasSelecionadas,
            List<int> quantidades)
        {
            if (User.IsInRole("Cliente"))
            {
                encomenda.Estado = Estados.Pendente;
            }

            if (quantidades == null || pecasSelecionadas.Count == 0)
            {
                ModelState.AddModelError("", "Selecione pelo menos uma peça");
            }
            else
            {
                try
                {
                    List<Pecas> pecas = new();

                    // Se for CLiente apenas pode criar encomendas com as suas Pecas
                    if (User.IsInRole("Cliente")) 
                    {

                        var userId = _userManager.GetUserId(User);
                        var user = await _userManager.GetUserAsync(User);
                        var nif = user.NIF;

                        pecas = await _context.Pecas
                            .Where(p => p.ClienteId == nif && pecasSelecionadas.Contains(p.Id))
                            .ToListAsync();
                    }
                    else if (User.IsInRole("Funcionario") || User.IsInRole("Admin")) // Se for Funcionario ou Admin pode criar encomendas com Pecas associadas ao cliente colocado
                    {
                        pecas = await _context.Pecas
                            .Where(p => pecasSelecionadas.Contains(p.Id))
                            .ToListAsync();
                    }

                    if (pecas.Count == 0)
                    {
                        ModelState.AddModelError("", "Não foram encontradas peças válidas para a encomenda.");
                        return View(encomenda);
                    }

                }
                catch
                {
                    ModelState.AddModelError("", "Erro no cálculo dos valores");
                }
            }

            // Validação server side
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
                    TempData["SuccessMessage"] = "Encomenda criada com sucesso!";

                    // Obter dados da encomenda quando criada
                    var cliente = await _context.Clientes.FindAsync(encomenda.ClienteId);
                    var nomeCliente = cliente.Nome ?? "Cliente Desconhecido";

                    // Notificar os Funcionarios com o Signal R
                    await _hubcontext.Clients.Group("Funcionarios").SendAsync("ReceberNotificacao", new
                    {
                        idEncomenda = encomenda.Id,
                        nomeCliente,
                        data = encomenda.Data.ToString("dd/MM/yyyy"),
                        precoTotal = encomenda.TotalPrecoUnit,
                        quantidadeTotal = encomenda.QuantidadeTotal
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }
            else
            {
                // Adicionado error para debuging
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
            }

            return View(encomenda);
        }

        // Formulário de edição de uma encomenda
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var encomenda = await _context.Encomendas
                .Include(e => e.Cliente)
                .Include(e => e.LinhasEncomenda)
                .ThenInclude(le => le.Peca)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (encomenda == null) return NotFound();

            ViewData["Estado"] = EstadosTraduzidosHelper.GetEstadosTraduzidos();
            return View(encomenda);
        }

        // Submissão do formulario de edição
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
        [Bind("Id,IdLadoCliente,Data,Transportadora,Estado,ClienteId,TotalPrecoUnit,QuantidadeTotal")] Encomendas encomenda,
        // Lista de ids das peças
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


        // Confirmação da eliminação de um cliente
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var encomenda = await _context.Encomendas
                .Include(e => e.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);

            ViewBag.estadoEncomenda = EstadosTraduzidosHelper.GetUmEstadoTraduzido(encomenda.Estado.ToString());

            return encomenda == null ? NotFound() : View(encomenda);
        }

        // Eliminação efetiva de um cliente
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

        /// <summary>
        /// Buscar as pecas associadas ao Cliente.
        /// Caso o Cliente esteja autenticado, busca o seu NIf e retorna as pecas associadas ao cliente NIf correspondente
        /// </summary>
        /// <param name="clienteId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPecasPorCliente(string clienteId)
        {

            var pecas = await _context.Pecas
                .Where(p => p.ClienteId == clienteId)
                .Select(p => new
                {
                    id = p.Id,
                    nome = p.Designacao,
                    preco = p.PrecoUnit.ToString(CultureInfo.InvariantCulture),
                    imagem = "/images/" + p.Imagem
                })
                .ToListAsync();

            return Json(pecas);
        }

        /// <summary>
        /// Buscar as encomendas associadas ao utilizador autenticado.
        /// </summary>
        /// <returns>Lista de Encomendas</returns>
        public async Task<IActionResult> GetClientesEncomendas()
        {
            var userId = _userManager.GetUserId(User);

            var encomendas = await _context.Encomendas
                .Where(e => e.ClienteId == userId)
                .Include(e => e.LinhasEncomenda)
                .ToListAsync();

            return View(encomendas);
        }

        
    }
}