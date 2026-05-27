using Microsoft.EntityFrameworkCore;
using ClinicaEstetica.Data;
using ClinicaEstetica.Models;

namespace ClinicaEstetica.Repositories;

public class AgendamentoRepository
{
    private readonly AppDbContext _context;

    public AgendamentoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Agendamento>> GetAllAsync(
        DateTime? data,
        int? biomedicoId,
        int? clienteId,
        StatusAgendamento? status)
    {
        var query = _context.Agendamentos
            .Include(a => a.Biomedico)
            .Include(a => a.Cliente)
            .Include(a => a.Procedimento)
            .AsQueryable();

        if (data.HasValue)
            query = query.Where(a => a.DataHora.Date == data.Value.Date);

        if (biomedicoId.HasValue)
            query = query.Where(a => a.BiomedicoId == biomedicoId.Value);

        if (clienteId.HasValue)
            query = query.Where(a => a.ClienteId == clienteId.Value);

        if (status.HasValue)
            query = query.Where(a => a.Status == status.Value);

        return await query
            .OrderBy(a => a.DataHora)
            .ToListAsync();
    }

    public async Task<Agendamento?> GetByIdAsync(int id)
    {
        return await _context.Agendamentos
            .Include(a => a.Biomedico)
            .Include(a => a.Cliente)
            .Include(a => a.Procedimento)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    // Verifica se biomédico ou cliente já têm agendamento no mesmo horário
    public async Task<bool> TemConflitoAsync(
        int biomedicoId,
        int clienteId,
        DateTime dataHora,
        int duracaoMinutos,
        int? ignorarAgendamentoId = null)
    {
        var fim = dataHora.AddMinutes(duracaoMinutos);

        var query = _context.Agendamentos
            .Include(a => a.Procedimento)
            .Where(a => a.Status != StatusAgendamento.Cancelado);

        if (ignorarAgendamentoId.HasValue)
            query = query.Where(a => a.Id != ignorarAgendamentoId.Value);

        return await query.AnyAsync(a =>
            (a.BiomedicoId == biomedicoId || a.ClienteId == clienteId) &&
            a.DataHora < fim &&
            a.DataHora.AddMinutes(a.Procedimento.DuracaoMinutos) > dataHora
        );
    }

    public async Task<Agendamento> CreateAsync(Agendamento agendamento)
    {
        _context.Agendamentos.Add(agendamento);
        await _context.SaveChangesAsync();
        return agendamento;
    }

    public async Task<Agendamento> UpdateAsync(Agendamento agendamento)
    {
        _context.Agendamentos.Update(agendamento);
        await _context.SaveChangesAsync();
        return agendamento;
    }

    public async Task<List<Agendamento>> GetResumoDiaAsync(DateTime data)
    {
        return await _context.Agendamentos
            .Include(a => a.Biomedico)
            .Include(a => a.Cliente)
            .Include(a => a.Procedimento)
            .Where(a => a.DataHora.Date == data.Date)
            .OrderBy(a => a.DataHora)
            .ToListAsync();
    }
}