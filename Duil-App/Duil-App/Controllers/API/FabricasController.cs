using Duil_App.Data;
using Duil_App.Models;
using Duil_App.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Duil_App.Controllers.API
{
    [Authorize(Roles = "Admin,Funcionario")]
    [Route("api/[controller]")]
    [ApiController]
    public class FabricasController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FabricasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<FabricasController>
        /// <summary>
        /// Devolve uma lista com todas as Fabricas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fabricas>>> GetFabricas()
        {
            return await _context.Fabricas.ToListAsync();
        }

        // GET api/<FabricasController>/5
        /// <summary>
        /// Obtem uma Fabrica especifica
        /// </summary>
        /// <param name="nif">identificador da Fabrica</param>
        /// <returns>Fabrica com o ID passado</returns>
        [HttpGet("{nif}")]
        public async Task<ActionResult<Fabricas>> GetFabrica(string nif)
        {
            var fabrica = await _context.Fabricas.FindAsync(nif);

            if (fabrica == null)
            {
                return NotFound();
            }

            return fabrica;
        }


        // POST api/<FabricasController>
        /// <summary>
        /// Adiciona uma fabrica
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Fabricas>> PostFabrica(FabricaDTO dto)
        {

            var novaFabrica = new Fabricas
            {
                Nif = dto.Nif,
                Nome = dto.Nome,
                Pais = dto.Pais,
                Morada = dto.Morada,
                CodPostal = dto.CodPostal,
                Telemovel = dto.Telemovel,
                Email = dto.Email,
                MoradaDescarga = dto.MoradaDescarga    
            };


            _context.Fabricas.Add(novaFabrica);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFabrica),
                       new { id = novaFabrica.Nif },
                       novaFabrica);
        }


        // PUT api/<FabricasController>/5
        /// <summary>
        /// Modifica uma fabrica
        /// </summary>
        /// <param name="nif"></param>
        /// <param name="dto"></param>
        [HttpPut("{nif}")]
        public async Task<IActionResult> PutFabrica(string nif, FabricaDTO dto)
        {
            var fabrica = await _context.Fabricas.FindAsync(nif);

            if (fabrica == null)
            {
                return BadRequest();
            }

            // Atualizar apenas os campos permitidos
            fabrica.Nif = dto.Nif;
            fabrica.Nome = dto.Nome;
            fabrica.Pais = dto.Pais;
            fabrica.Morada = dto.Morada;
            fabrica.CodPostal = dto.CodPostal;
            fabrica.Telemovel = dto.Telemovel;
            fabrica.Email = dto.Email;
            fabrica.MoradaDescarga = dto.MoradaDescarga;

            _context.Entry(fabrica).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Erro de concorrência ao atualizar a fabrica.");
            }

            return NoContent();
        }

        // DELETE api/<FabricasController>/5
        /// <summary>
        /// Apaga uma fabrica
        /// </summary>
        /// <param name="nif"></param>
        [HttpDelete("{nif}")]
        public async Task<IActionResult> DeleteFabrica(string nif)
        {
            var fabrica = await _context.Fabricas.FindAsync(nif);
            if (fabrica == null)
            {
                return NotFound();
            }

            _context.Fabricas.Remove(fabrica);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}

