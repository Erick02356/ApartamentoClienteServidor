﻿using ApiApartamentos.DTOs;
using ApiApartamentos.Hubs;
using AutoMapper;
using DataAccessLayer.Models;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ApiApartamentos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartamentoController : ControllerBase
    {
        private readonly IApartamentoRepository<Apartamento> _apartamentoRepository;
        private readonly IHubContext<ApartamentosHub> _hubContext;
        private readonly IMapper _mapper;

        public ApartamentoController(IApartamentoRepository<Apartamento> apartamentoRepository, IHubContext<ApartamentosHub> hubContext, IMapper mapper)
        {
            _apartamentoRepository = apartamentoRepository;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        [HttpGet("cliente/{tipoCliente}")]
        public async Task<ActionResult<IEnumerable<object>>> GetApartamentos(string tipoCliente = "web")
        {
            var apartamentos = await _apartamentoRepository.GetAll();

            if (tipoCliente.ToLower() == "winforms")
                return Ok(_mapper.Map<List<ApartamentoFullDTO>>(apartamentos));

            return Ok(_mapper.Map<List<ApartamentoDTO>>(apartamentos));
        }
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var apartamentos = await _apartamentoRepository.GetAll();
        //    return Ok(apartamentos);
        //}

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

            await _hubContext.Clients.All.SendAsync("RecargarDatos");

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

            await _hubContext.Clients.All.SendAsync("RecargarDatos");

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

            await _hubContext.Clients.All.SendAsync("RecargarDatos");

            return NoContent(); // Retorna 204 si la eliminación fue exitosa
        }

        [HttpGet("usuario/{usuarioResponsable}")]
        public async Task<IActionResult> GetByUSer(string usuarioResponsable)
        {
            var apartamento = await _apartamentoRepository.GetByUserAsync(usuarioResponsable);
            if (apartamento == null)
                return NotFound();

            return Ok(apartamento);
        }
    }
}
