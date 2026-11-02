using Microsoft.AspNetCore.Mvc;
using RegistroEstudiantes.Data.Models;
using RegistroEstudiantes.Data.Services;
using Mapster;
using RegistroEstudiantes.api.DTOs;

namespace RegistroEstudiantes.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LibrosController(LibrosService librosService) : ControllerBase
{
    // GET: api/<LibrosController>
    [HttpGet]
    public async Task<ActionResult<List<LibroDTO>>> Get()
    {
        var librosResult = await librosService.GetList(l => true);

        if (librosResult.IsFailure)
            return BadRequest(librosResult.Error);

        var librosDTO = librosResult.Value.Adapt<List<LibroDTO>>();

        return Ok(librosDTO);
    }

    // GET api/<LibrosController>/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<LibroDTO>> Get(int id)
    {
        var libroResult = await librosService.Buscar(id);

        if (libroResult.IsFailure)
            return BadRequest(libroResult.Error);

        if (libroResult.Value == null)
            return NotFound();

        var libroDTO = libroResult.Value.Adapt<LibroDTO>();

        return Ok(libroDTO);
    }

    // POST api/<LibrosController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] LibroDTO libroDTO)
    {
        var libro = libroDTO.Adapt<Libro>();

        libro.LibroId = 0;

        var resultado = await librosService.Guardar(libro);

        if (resultado.IsFailure)
            return BadRequest(resultado.Error);

        return Created();
    }

    // PUT api/<LibrosController>/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] LibroDTO libroDTO)
    {
        if (id != libroDTO.LibroId)
            return BadRequest();

        var libroActual = await librosService.Buscar(id);

        if (libroActual.IsFailure)
            return BadRequest(libroActual.Error);

        if (libroActual.Value == null)
            return NotFound();

        var libro = libroDTO.Adapt<Libro>();

        var resultado = await librosService.Guardar(libro);

        if (resultado.IsFailure)
            return BadRequest(resultado.Error);

        return NoContent();
    }

    // DELETE api/<LibrosController>/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var libro = await librosService.Buscar(id);

        if (libro.IsFailure)
            return BadRequest(libro.Error);

        if (libro.Value == null)
            return NotFound();

        var resultado = await librosService.Eliminar(id);

        if (resultado.IsFailure)
            return BadRequest(resultado.Error);

        return NoContent();
    }
}
