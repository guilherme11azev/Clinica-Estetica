using Microsoft.EntityFrameworkCore;
using ClinicaEstetica.Data;
using ClinicaEstetica.Models;

namespace ClinicaEstetica.Repositories;

public class ProcedimentoRepository
{
    private readonly AppDbContext _context;

    public ProcedimentoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Procedimento>> GetAllAsync()
    {
        return await _context.Procedimentos
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    public async Task<Procedimento?> GetByIdAsync(int id)
    {
        return await _context.Procedimentos.FindAsync(id);
    }

    public async Task<Procedimento> CreateAsync(Procedimento procedimento)
    {
        _context.Procedimentos.Add(procedimento);
        await _context.SaveChangesAsync();
        return procedimento;
    }

    public async Task<Procedimento> UpdateAsync(Procedimento procedimento)
    {
        _context.Procedimentos.Update(procedimento);
        await _context.SaveChangesAsync();
        return procedimento;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var procedimento = await GetByIdAsync(id);
        if (procedimento == null) return false;

        _context.Procedimentos.Remove(procedimento);
        await _context.SaveChangesAsync();
        return true;
    }
}