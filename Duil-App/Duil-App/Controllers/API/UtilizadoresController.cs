using Duil_App.Data;
using Duil_App.Models;
using Duil_App.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Duil_App.Controllers.API
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;

        public UtilizadoresController(UserManager<Utilizadores> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
             _context = context;        
        }

        // GET: api/<UtilizadoresController>
        /// <summary>
        /// Devolve uma lista com todas as Utilizadores
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilizadores>>> GetUtilizadores()
        {
            return await _context.Utilizadores.ToListAsync();
        }

        // GET api/<UtilizadoresController>/5
        /// <summary>
        /// Obtem um Utilizador especifico
        /// </summary>
        /// <param name="nif">identificador de Utilizador</param>
        /// <returns>Utilizador com o ID passado</returns>
        [HttpGet("{nif}")]
        public async Task<ActionResult<Utilizadores>> GetUtilizador(string nif)
        {
            var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.NIF == nif);

            if (utilizador == null)
            {
                return NotFound();
            }

            return utilizador;
        }


        // POST api/<UtilizadoresController>
        /// <summary>
        /// Adiciona um utilizador
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Utilizadores>> PostUtilizador(UtilizadorDTO dto)
        {

            var novoUtilizador = new Utilizadores
            {
                Nome = dto.Nome,
                CodPostal = dto.CodPostal,
                Morada = dto.Morada,
                Pais = dto.Pais,
                NIF = dto.NIF,
                Telemovel = dto.Telemovel,
                Email = dto.Email,
                UserName = dto.Email
            };


            var result = await _userManager.CreateAsync(novoUtilizador, dto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(novoUtilizador, "Utilizador");

                return Ok(new { mensagem = "Utilizador criado com sucesso." });
            }

            // Se falhar, devolve os erros
            return BadRequest(result.Errors);
        }


        // PUT api/<UtilizadoresController>/5
        /// <summary>
        /// Modifica um Utilizador
        /// </summary>
        /// <param name="nif"></param>
        /// <param name="dto"></param>
        [HttpPut("{nif}")]
        public async Task<IActionResult> PutUtilizador(string nif, UtilizadorDTO dto)
        {
            var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.NIF == nif);

            if (utilizador == null)
            {
                return BadRequest();
            }

            // Atualizar apenas os campos permitidos
            utilizador.Nome = dto.Nome;
            utilizador.Pais = dto.Pais;
            utilizador.NIF = dto.NIF;
            utilizador.Morada = dto.Morada;
            utilizador.CodPostal = dto.CodPostal;
            utilizador.Telemovel = dto.Telemovel;
            utilizador.Email = dto.Email;


            _context.Entry(utilizador).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Erro de concorrência ao atualizar a utilizador.");
            }

            return NoContent();
        }

        // DELETE api/<UtilizadoresController>/5
        /// <summary>
        /// Apaga um utilizador
        /// </summary>
        /// <param name="nif"></param>
        [HttpDelete("{nif}")]
        public async Task<IActionResult> DeleteUtilizador(string nif)
        {
            var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.NIF == nif);
            if (utilizador == null)
            {
                return NotFound();
            }

            _context.Utilizadores.Remove(utilizador);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}

