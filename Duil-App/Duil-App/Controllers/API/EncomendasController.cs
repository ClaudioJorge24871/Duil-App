using Duil_App.Data;
using Duil_App.Models;
using Duil_App.Models.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Duil_App.Controllers.API
{

    [Route("api/[controller]")]
    [ApiController]
    public class EncomendasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;

        public EncomendasController(ApplicationDbContext context, UserManager<Utilizadores> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: api/<EncomendasController>
        /// <summary>
        /// Devolve uma lista com todas as Encomendas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Encomendas>>> GetEncomendas()
        {
            return await _context.Encomendas.ToListAsync();
        }

        // GET api/<EncomendasController>/5
        /// <summary>
        /// Obtem uma Encomenda especifica
        /// </summary>
        /// <param name="id">identificador da Encomenda</param>
        /// <returns>Encomenda com o ID passado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Encomendas>> GetEncomenda(int id)
        {
            var encomenda = await _context.Encomendas.FindAsync(id);

            if (encomenda == null)
            {
                return NotFound();
            }

            return encomenda;
        }


        // POST api/<EncomendasController>
        /// <summary>
        /// Adiciona uma encomenda
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Encomendas>> PostEncomenda(EncomendaDTO dto)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Nif == dto.ClienteId);

            var novaEncomenda = new Encomendas
            {
                IdLadoCliente   = dto.IdLadoCliente,
                ClienteId       = dto.ClienteId,
                Cliente         = cliente ?? throw new ArgumentNullException(nameof(cliente), "Cliente não encontrado."),
                Data            = DateTime.Today,
                Estado          = Estados.Pendente,
                TotalPrecoUnit  = 1,                    //Duvida aqui.
                QuantidadeTotal = 1,                   // e aqui
                Transportadora  = string.Empty
            };


            _context.Encomendas.Add(novaEncomenda);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEncomenda),
                       new { id = novaEncomenda.Id },
                       novaEncomenda);
        }


        // GET: api/Encomendas/minhasEncomendas
        /// <summary>
        /// Devolve uma lista com todas as Encomendas do cliente autenticado
        /// </summary>
        [HttpGet("minhasEncomendas")]
        [Authorize(Roles = "Cliente")]
        public async Task<ActionResult<IEnumerable<Encomendas>>> VerMinhas()
        {
            var userNif = _userManager.Users
                .Where(u => u.Id == _userManager.GetUserId(User))
                .Select(u => u.NIF)
                .FirstOrDefault();
            
            return await _context.Encomendas
                .Where(e => e.ClienteId == userNif)
                .ToListAsync();
        }

        // POST: api/Encomendas/cliente
        /// <summary>
        /// Cria a encomenda do cliente autenticado
        /// </summary>
        /// <param name="dto"></param>
        [HttpPost("cliente")]
        [Authorize(Roles = "Cliente")]
        public async Task<ActionResult<Encomendas>> CriarEncomendaCliente([FromBody] EncomendaClienteDTO dto)
        {
            var userNif = _userManager.Users
               .Where(u => u.Id == _userManager.GetUserId(User))
               .Select(u => u.NIF)
               .FirstOrDefault();

            var novaEncomenda = new Encomendas
            {
                IdLadoCliente = dto.IdLadoCliente,
                //Como é autenticado, existe
                ClienteId = userNif!,
                Cliente = _context.Clientes.FirstOrDefault(e => e.Nif == userNif)!,
                Data = DateTime.Today,
                Estado = Estados.Pendente,
                TotalPrecoUnit = 1,                    //Duvida aqui.
                QuantidadeTotal = 1,                   // e aqui
                Transportadora = dto.Transportadora
            };

            _context.Encomendas.Add(novaEncomenda);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(VerMinhas), new { id = novaEncomenda.Id }, novaEncomenda);
        }


        // PUT api/<EncomendasController>/5
        /// <summary>
        /// Modifica uma encomenda
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEncomenda(int id, EncomendaDTO dto)
        {
            var encomenda = await _context.Encomendas.FindAsync(id);
           
            if (encomenda == null)
            {
                return BadRequest();
            }

            // Atualizar apenas os campos permitidos
            encomenda.ClienteId = dto.ClienteId;
            encomenda.Transportadora = dto.Transportadora;
            encomenda.IdLadoCliente = dto.IdLadoCliente;

            _context.Entry(encomenda).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Erro de concorrência ao atualizar a encomenda.");
            }

            return NoContent();
        }

        // DELETE api/<EncomendasController>/5
        /// <summary>
        /// Apaga uma encomenda
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEncomenda(int id)
        {
            var encomenda = await _context.Encomendas.FindAsync(id);
            if (encomenda == null)
            {
                return NotFound();
            }

            _context.Encomendas.Remove(encomenda);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
