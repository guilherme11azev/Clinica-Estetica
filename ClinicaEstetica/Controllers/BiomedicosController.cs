using Microsoft.AspNetCore.Mvc;
using ClinicaEstetica.DTOs;
using ClinicaEstetica.Services;

namespace ClinicaEstetica.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BiomedicosController : ControllerBase
{
    private readonly BiomedicoService _service;

    public BiomedicosController(BiomedicoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var biomedicos = await _service.GetAllAsync();
        return Ok(biomedicos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var biomedico = await _service.GetByIdAsync(id);
        if (biomedico == null) return NotFound("Biomédico não encontrado");
        return Ok(biomedico);
    }

    [HttpGet("{id}/agenda")]
    public async Task<IActionResult> GetAgenda(int id, [FromQuery] DateTime inicio, [FromQuery] DateTime fim)
    {
        try
        {
            var agenda = await _service.GetAgendaAsync(id, inicio, fim);
            return Ok(agenda);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BiomedicoCreateDTO dto)
    {
        var criado = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BiomedicoUpdateDTO dto)
    {
        var atualizado = await _service.UpdateAsync(id, dto);
        if (atualizado == null) return NotFound("Biomédico não encontrado");
        return Ok(atualizado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var removido = await _service.DeleteAsync(id);
        if (!removido) return NotFound("Biomédico não encontrado");
        return NoContent();
    }
}