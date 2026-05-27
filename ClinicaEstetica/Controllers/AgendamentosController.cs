using Microsoft.AspNetCore.Mvc;
using ClinicaEstetica.DTOs;
using ClinicaEstetica.Models;
using ClinicaEstetica.Services;

namespace ClinicaEstetica.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgendamentosController : ControllerBase
{
    private readonly AgendamentoService _service;

    public AgendamentosController(AgendamentoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] DateTime? data,
        [FromQuery] int? biomedicoId,
        [FromQuery] int? clienteId,
        [FromQuery] StatusAgendamento? status)
    {
        var agendamentos = await _service.GetAllAsync(data, biomedicoId, clienteId, status);
        return Ok(agendamentos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var agendamento = await _service.GetByIdAsync(id);
        if (agendamento == null) return NotFound("Agendamento não encontrado");
        return Ok(agendamento);
    }

    [HttpGet("resumo-dia")]
    public async Task<IActionResult> GetResumoDia([FromQuery] DateTime data)
    {
        var resumo = await _service.GetResumoDiaAsync(data);
        return Ok(resumo);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AgendamentoCreateDTO dto)
    {
        try
        {
            var criado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPatch("{id}/confirmar")]
    public async Task<IActionResult> Confirmar(int id)
    {
        try
        {
            var agendamento = await _service.ConfirmarAsync(id);
            return Ok(agendamento);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPatch("{id}/concluir")]
    public async Task<IActionResult> Concluir(int id)
    {
        try
        {
            var agendamento = await _service.ConcluirAsync(id);
            return Ok(agendamento);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPatch("{id}/cancelar")]
    public async Task<IActionResult> Cancelar(int id, [FromBody] AgendamentoCancelarDTO dto)
    {
        try
        {
            var agendamento = await _service.CancelarAsync(id, dto);
            return Ok(agendamento);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}