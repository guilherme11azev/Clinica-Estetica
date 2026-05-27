using Microsoft.AspNetCore.Mvc;
using ClinicaEstetica.DTOs;
using ClinicaEstetica.Services;

namespace ClinicaEstetica.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProcedimentosController : ControllerBase
{
    private readonly ProcedimentoService _service;

    public ProcedimentosController(ProcedimentoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var procedimentos = await _service.GetAllAsync();
        return Ok(procedimentos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var procedimento = await _service.GetByIdAsync(id);
        if (procedimento == null) return NotFound("Procedimento não encontrado");
        return Ok(procedimento);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProcedimentoCreateDTO dto)
    {
        var criado = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProcedimentoUpdateDTO dto)
    {
        var atualizado = await _service.UpdateAsync(id, dto);
        if (atualizado == null) return NotFound("Procedimento não encontrado");
        return Ok(atualizado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var removido = await _service.DeleteAsync(id);
        if (!removido) return NotFound("Procedimento não encontrado");
        return NoContent();
    }
}