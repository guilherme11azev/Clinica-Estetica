using Microsoft.EntityFrameworkCore;
using ClinicaEstetica.Data;
using ClinicaEstetica.Models;

namespace ClinicaEstetica.Repositories;

public class BiomedicoRepository
{
    private readonly AppDbContext _context;

    public BiomedicoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Biomedico>> GetAllAsync()
    {
        return await _context.Biomedicos
            .OrderBy(b => b.Nome)
            .ToListAsync();
    }

    public async Task<Biomedico?> GetByIdAsync(int id)
    {
        return await _context.Biomedicos.FindAsync(id);
    }

    public async Task<List<Agendamento>> GetAgendaAsync(int biomedicoId, DateTime inicio, DateTime fim)
    {
        return await _context.Agendamentos
            .Include(a => a.Cliente)
            .Include(a => a.Procedimento)
            .Where(a => a.BiomedicoId == biomedicoId
                     && a.DataHora >= inicio
                     && a.DataHora <= fim
                     && a.Status != StatusAgendamento.Cancelado)
            .OrderBy(a => a.DataHora)
            .ToListAsync();
    }

    public async Task<Biomedico> CreateAsync(Biomedico biomedico)
    {
        _context.Biomedicos.Add(biomedico);
        await _context.SaveChangesAsync();
        return biomedico;
    }

    public async Task<Biomedico> UpdateAsync(Biomedico biomedico)
    {
        _context.Biomedicos.Update(biomedico);
        await _context.SaveChangesAsync();
        return biomedico;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var biomedico = await GetByIdAsync(id);
        if (biomedico == null) return false;

        _context.Biomedicos.Remove(biomedico);
        await _context.SaveChangesAsync();
        return true;
    }
}