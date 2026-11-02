using Microsoft.AspNetCore.Mvc;
using RegistroEstudiantes.Data.Models;
using RegistroEstudiantes.Data.Services;
using Mapster;
using RegistroEstudiantes.api.DTOs;

namespace RegistroEstudiantes.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstudiantesController(EstudiantesService estudiantesService) : ControllerBase
{
    // GET: api/<EstudiantesController>
    [HttpGet]
    public async Task<ActionResult<List<EstudianteDTO>>> Get()
    {
        var estudiantesResult = await estudiantesService.GetList(e => true);

        if (estudiantesResult.IsFailure)
            return BadRequest(estudiantesResult.Error);

        var estudiantesDTO = estudiantesResult.Value.Adapt<List<EstudianteDTO>>();

        return Ok(estudiantesDTO);
    }

    // GET api/<EstudiantesController>/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<EstudianteDTO>> Get(int id)
    {
        var estudianteResult = await estudiantesService.Buscar(id);

        if (estudianteResult.IsFailure)
            return BadRequest(estudianteResult.Error);

        if (estudianteResult.Value == null)
            return NotFound();

        var estudianteDTO = estudianteResult.Value.Adapt<EstudianteDTO>();

        return Ok(estudianteDTO);
    }

    // POST api/<EstudiantesController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] EstudianteDTO estudianteDTO)
    {
        var estudiante = estudianteDTO.Adapt<Estudiante>();

        estudiante.EstudianteId = 0;

        var resultado = await estudiantesService.Guardar(estudiante);

        if (resultado.IsFailure)
            return BadRequest(resultado.Error);

        return Created();
    }

    // PUT api/<EstudiantesController>/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] EstudianteDTO estudianteDTO)
    {
        if (id != estudianteDTO.EstudianteId)
            return BadRequest();

        var estudianteActual = await estudiantesService.Buscar(id);

        if (estudianteActual.IsFailure)
            return BadRequest(estudianteActual.Error);

        if (estudianteActual.Value == null)
            return NotFound();

        var estudiante = estudianteDTO.Adapt<Estudiante>();

        var resultado = await estudiantesService.Guardar(estudiante);

        if (resultado.IsFailure)
            return BadRequest(resultado.Error);

        return NoContent();
    }

    // DELETE api/<EstudiantesController>/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var estudiante = await estudiantesService.Buscar(id);

        if (estudiante.IsFailure)
            return BadRequest(estudiante.Error);

        if (estudiante.Value == null)
            return NotFound();

        var resultado = await estudiantesService.Eliminar(id);

        if (resultado.IsFailure)
            return BadRequest(resultado.Error);

        return NoContent();
    }
}
