using Microsoft.AspNetCore.Mvc;
using RegistroEstudiantes.Data.Models;
using RegistroEstudiantes.Data.Services;
using Mapster;
using RegistroEstudiantes.api.DTOs;

namespace RegistroEstudiantes.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrestamosController(PrestamosService prestamosService) : ControllerBase
{
    // GET: api/<PrestamosController>
    [HttpGet]
    public async Task<ActionResult<List<PrestamoDTO>>> Get()
    {
        var prestamosResult = await prestamosService.GetList(p => true);

        if (prestamosResult.IsFailure)
            return BadRequest(prestamosResult.Error);

        var prestamosDTO = prestamosResult.Value.Adapt<List<PrestamoDTO>>();

        return Ok(prestamosDTO);
    }

    // GET api/<PrestamosController>/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PrestamoDTO>> Get(int id)
    {
        var prestamoResult = await prestamosService.Buscar(id);

        if (prestamoResult.IsFailure)
            return BadRequest(prestamoResult.Error);

        if (prestamoResult.Value == null)
            return NotFound();

        var prestamoDTO = prestamoResult.Value.Adapt<PrestamoDTO>();

        return Ok(prestamoDTO);
    }

    // POST api/<PrestamosController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PrestamoDTO prestamoDTO)
    {
        var prestamo = prestamoDTO.Adapt<Prestamo>();

        prestamo.PrestamoId = 0;

        var resultado = await prestamosService.Guardar(prestamo);

        if (resultado.IsFailure)
            return BadRequest(resultado.Error);

        return Created();
    }

    // PUT api/<PrestamosController>/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] PrestamoDTO prestamoDTO)
    {
        if (id != prestamoDTO.PrestamoId)
            return BadRequest();

        var prestamoActual = await prestamosService.Buscar(id);

        if (prestamoActual.IsFailure)
            return BadRequest(prestamoActual.Error);

        if (prestamoActual.Value == null)
            return NotFound();

        var prestamo = prestamoDTO.Adapt<Prestamo>();

        var resultado = await prestamosService.Guardar(prestamo);

        if (resultado.IsFailure)
            return BadRequest(resultado.Error);

        return NoContent();
    }

    // DELETE api/<PrestamosController>/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var prestamo = await prestamosService.Buscar(id);

        if (prestamo.IsFailure)
            return BadRequest(prestamo.Error);

        if (prestamo.Value == null)
            return NotFound();

        var resultado = await prestamosService.Eliminar(id);

        if (resultado.IsFailure)
            return BadRequest(resultado.Error);

        return NoContent();
    }
}
