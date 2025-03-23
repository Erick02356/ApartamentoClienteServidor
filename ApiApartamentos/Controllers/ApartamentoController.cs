using DataAccessLayer.Models;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiApartamentos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartamentoController : ControllerBase
    {
        private readonly IApartamentoRepository<Apartamento> _apartamentoRepository;

        public ApartamentoController(IApartamentoRepository<Apartamento> apartamentoRepository)
        {
            _apartamentoRepository = apartamentoRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var apartamentos = await _apartamentoRepository.GetAll();
            return Ok(apartamentos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var apartamento = await _apartamentoRepository.GetEntityById(id);
            if (apartamento == null)
            {
                return NotFound();
            }
            return Ok(apartamento);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Apartamento entity)
        {
            if (entity == null)
            {
                return BadRequest("Datos inválidos");
            }
            var created = await _apartamentoRepository.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new {id = created.ApartamentoId}, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Apartamento entity)
        {
            if (entity == null || id != entity.ApartamentoId)
                return BadRequest("Datos inválidos");
            var updated = await _apartamentoRepository.UpdateAsync(entity);
            if (updated == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var apartamento = await _apartamentoRepository.GetEntityById(id);
            if (apartamento == null)
            {
                return NotFound(); // Retorna 404 si no existe
            }

            await _apartamentoRepository.DeleteAsync(id);
            return NoContent(); // Retorna 204 si la eliminación fue exitosa
        }
    }
}
