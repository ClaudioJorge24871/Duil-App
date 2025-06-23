using Duil_App.Data;
using Duil_App.Models;
using Duil_App.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Duil_App.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<ClientesController>
        /// <summary>
        /// Devolve uma lista com todas as Clientes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clientes>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        // GET api/<ClientesController>/5
        /// <summary>
        /// Obtem um Cliente especifico
        /// </summary>
        /// <param name="nif">identificador de Cliente</param>
        /// <returns>Cliente com o ID passado</returns>
        [HttpGet("{nif}")]
        public async Task<ActionResult<Clientes>> GetCliente(string nif)
        {
            var cliente = await _context.Clientes.FindAsync(nif);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }


        // POST api/<ClientesController>
        /// <summary>
        /// Adiciona um cliente
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Clientes>> PostCliente(ClienteDTO dto)
        {

            var novoCliente = new Clientes
            {
                Nif = dto.Nif,
                Nome = dto.Nome,
                Pais = dto.Pais,
                Morada = dto.Morada,
                CodPostal = dto.CodPostal,
                Telemovel = dto.Telemovel,
                Email = dto.Email,
                MoradaCarga = dto.MoradaCarga
            };


            _context.Clientes.Add(novoCliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente),
                       new { id = novoCliente.Nif },
                       novoCliente);
        }


        // PUT api/<ClientesController>/5
        /// <summary>
        /// Modifica um Cliente
        /// </summary>
        /// <param name="nif"></param>
        /// <param name="dto"></param>
        [HttpPut("{nif}")]
        public async Task<IActionResult> PutCliente(string nif, ClienteDTO dto)
        {
            var cliente = await _context.Clientes.FindAsync(nif);

            if (cliente == null)
            {
                return BadRequest();
            }

            // Atualizar apenas os campos permitidos
            cliente.Nif = dto.Nif;
            cliente.Nome = dto.Nome;
            cliente.Pais = dto.Pais;
            cliente.Morada = dto.Morada;
            cliente.CodPostal = dto.CodPostal;
            cliente.Telemovel = dto.Telemovel;
            cliente.Email = dto.Email;
            cliente.MoradaCarga = dto.MoradaCarga;

            _context.Entry(cliente).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Erro de concorrência ao atualizar a cliente.");
            }

            return NoContent();
        }

        // DELETE api/<ClientesController>/5
        /// <summary>
        /// Apaga um cliente
        /// </summary>
        /// <param name="nif"></param>
        [HttpDelete("{nif}")]
        public async Task<IActionResult> DeleteCliente(string nif)
        {
            var cliente = await _context.Clientes.FindAsync(nif);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}

