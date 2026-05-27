using Microsoft.AspNetCore.Mvc;
using ClinicaEstetica.DTOs;
using ClinicaEstetica.Services;

namespace ClinicaEstetica.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly ClienteService _service;

    public ClientesController(ClienteService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var clientes = await _service.GetAllAsync();
        return Ok(clientes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var cliente = await _service.GetByIdAsync(id);
        if (cliente == null) return NotFound("Cliente não encontrado");
        return Ok(cliente);
    }

    [HttpGet("{id}/historico")]
    public async Task<IActionResult> GetHistorico(int id)
    {
        try
        {
            var historico = await _service.GetHistoricoAsync(id);
            return Ok(historico);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClienteCreateDTO dto)
    {
        var criado = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateDTO dto)
    {
        var atualizado = await _service.UpdateAsync(id, dto);
        if (atualizado == null) return NotFound("Cliente não encontrado");
        return Ok(atualizado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var removido = await _service.DeleteAsync(id);
        if (!removido) return NotFound("Cliente não encontrado");
        return NoContent();
    }
}