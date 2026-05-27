using Microsoft.EntityFrameworkCore;
using ClinicaEstetica.Data;
using ClinicaEstetica.Models;

namespace ClinicaEstetica.Repositories;

public class ClienteRepository
{
    private readonly AppDbContext _context;

    public ClienteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Cliente>> GetAllAsync()
    {
        return await _context.Clientes
            .OrderBy(c => c.Nome)
            .ToListAsync();
    }

    public async Task<Cliente?> GetByIdAsync(int id)
    {
        return await _context.Clientes.FindAsync(id);
    }

    public async Task<List<Agendamento>> GetHistoricoAsync(int clienteId)
    {
        return await _context.Agendamentos
            .Include(a => a.Biomedico)
            .Include(a => a.Procedimento)
            .Where(a => a.ClienteId == clienteId)
            .OrderByDescending(a => a.DataHora)
            .ToListAsync();
    }

    public async Task<Cliente> CreateAsync(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return cliente;
    }

    public async Task<Cliente> UpdateAsync(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync();
        return cliente;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cliente = await GetByIdAsync(id);
        if (cliente == null) return false;

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();
        return true;
    }
}