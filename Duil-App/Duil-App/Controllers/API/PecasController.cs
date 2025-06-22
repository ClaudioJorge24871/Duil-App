using Duil_App.Data;
using Duil_App.Models;
using Duil_App.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Duil_App.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PecasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PecasController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/<PecasController>
        /// <summary>
        /// Devolve uma lista com todas as Pecas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pecas>>> GetPecas()
        {
            return await _context.Pecas.ToListAsync();
        }

        // GET api/<PecasController>/5
        /// <summary>
        /// Obtem uma Peca em especifico
        /// </summary>
        /// <param name="id">identificador da Peca</param>
        /// <returns>Peca com o ID passado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Pecas>> GetPeca(int id)
        {
            var peca = await _context.Pecas.FindAsync(id);

            if (peca == null){
                return NotFound();
            };

            return peca;
        }


        // POST api/<PecasController>
        /// <summary>
        /// Adiciona uma peca
        /// </summary>
        /// <param name="peca"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Pecas>> PostPeca (PecaDTO dto)
        {
            var fabrica = await _context.Fabricas.FirstOrDefaultAsync(f => f.Nif == dto.FabricaId);
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Nif == dto.ClienteId);
            
            var novaPeca = new Pecas
            {
                Referencia = dto.Referencia,
                Designacao = dto.Designacao,
                PrecoUnit = dto.PrecoUnit,
                FabricaId = dto.FabricaId,
                Fabrica = fabrica ?? throw new ArgumentNullException(nameof(fabrica), "Fábrica não encontrada."),
                ClienteId = dto.ClienteId,
                Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente), "Cliente não encontrado."),
                Imagem = dto.Imagem
            };


            _context.Pecas.Add(novaPeca);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPeca),
                       new { id = novaPeca.Id },
                       novaPeca);
        }
        

        // PUT api/<PecasController>/5
        /// <summary>
        /// Modifica uma peca
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPeca(int id, Pecas peca)
        {
            if(id != peca.Id)
            {
                return BadRequest();
            }

            _context.Entry(peca).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }catch (DbUpdateConcurrencyException)
            {
                if (!_context.Pecas.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/<PecasController>/5
        /// <summary>
        /// Apaga uma peca
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePeca (int id)
        {
            var peca = await _context.Pecas.FindAsync(id);
            if (peca == null)
            {
                return NotFound();
            }

            _context.Pecas.Remove(peca);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
